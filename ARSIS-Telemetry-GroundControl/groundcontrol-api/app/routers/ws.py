from fastapi import APIRouter, WebSocket, WebSocketDisconnect, Request

import asyncio

router = APIRouter(prefix="/ws", tags=["ws"])


class WebSocketManager:
    def __init__(self):
        self.websockets = {}

    async def connect(self, path, ws):
        await ws.accept()
        if path not in self.websockets:
            self.websockets[path] = []
        self.websockets[path].append(ws)

    def disconnect(self, path, ws):
        self.websockets[path].remove(ws)

    async def async_receive(self, path):
        pass

    async def broadcast(self, ws, data):
        for ws_conn in self.websockets:
            if ws != ws_conn:
                await ws_conn.send_text(data)

    async def broadcast_to_all(self, data):
        for ws_conn in self.websockets:
            await ws_conn.send_text(data)
            print(data)


ws_manager = WebSocketManager()


@router.websocket("/events")
async def connect_to_events(websocket: WebSocket):
    await ws_manager.connect("events", websocket)
    try:
        while True:
            to_update = await websocket.app.state.datastore.get_updates()
            if len(to_update) > 0:
                print(to_update)
                for update in to_update:
                    await ws_manager.broadcast_to_all(update)
            await asyncio.sleep(0.1)
    except WebSocketDisconnect:
        ws_manager.disconnect(websocket)


@router.websocket("/updates")
async def connect_to_updates(websocket: WebSocket):
    await ws_manager.connect("updates", websocket)
    try:
        while True:
            data = await websocket.receive_text()
            await ws_manager.broadcast(websocket, data)
    except WebSocketDisconnect:
        ws_manager.disconnect(websocket)
