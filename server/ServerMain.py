import threading
import socket
import json
from DBApi import DatabaseManager
import os
from config.definitions import ROOT_DIR

server_run = True
server_listening = False

def end_server():
    global server_run
    while server_run:
        message = input("enter 'close' to stop server")
        if message == "close":
            host = "127.0.0.1"
            port = 5000

            client_socket = socket.socket()
            client_socket.connect((host, port))
            client_socket.close()
            server_run = False


def server_program():
    #global server_listening
    print("waiting for connection")
    host = "127.0.0.1"
    port = 5000

    server_socket = socket.socket()
    server_socket.bind((host, port))

    server_socket.listen(2)
    #server_listening = True
    conn, address = server_socket.accept()
    print("connection from: " + str(address))

    file_path = os.path.join(ROOT_DIR, "..\\passwords\\userpass.txt")
    pass_file = open(file_path, 'r')
    lines = pass_file.readlines()
    pass_file.close()
    db_accessor = DatabaseManager(lines[0].strip(), lines[1].strip())

    while True:
        data = conn.recv(1024).decode()
        if not data:
            break
        #print("from connected user: " + str(data))
        if str(data) == "get lifts":
            data = db_accessor.get_lift()#DatabaseManager.get_lift(lines[0].strip(), lines[1].strip())
        elif str(data) == "sent lifts":
            print('workout recieved')
        else:
            print(data)
        conn.send(data.encode())
    conn.close()

def send_lifts():
    return "deadlift 265 3x5"

def receive_lifts():
    pass

if __name__ == "__main__":
    #global server_listening
    t1 = threading.Thread(target=end_server, args=())
    t1.start()

    while server_run:
        t2 = threading.Thread(target=server_program, args=())
        t2.start()
        t2.join()

    t1.join()

    print("done")