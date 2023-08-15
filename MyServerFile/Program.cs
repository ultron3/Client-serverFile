// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;


TcpListener listener = new TcpListener(IPAddress.Any, 5550);
listener.Start();
Console.WriteLine("Server avviato. In attesa di connessioni...");

while (true)
{
    TcpClient client = listener.AcceptTcpClient();
    Console.WriteLine("Cliente connesso.");

    Thread clientThread = new Thread(HandleClient);
    clientThread.Start(client);
}

void HandleClient(object clientObj)
{
    TcpClient client = (TcpClient)clientObj;

    using (NetworkStream networkStream = client.GetStream())
    {
        byte[] buffer = new byte[1024];
        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);

        string fileName = "program.exe"; // Nome temporaneo del file ricevuto
        using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
        {
            fileStream.Write(buffer, 0, bytesRead);
        }

        // Simuliamo la trasformazione in .inv
        string transformedFileName = Path.ChangeExtension(fileName, ".inv");
        File.Move(fileName, transformedFileName);

        Console.WriteLine("File trasformato in .inv: " + transformedFileName);

        networkStream.Close();
    }

    client.Close();
    Console.WriteLine("Cliente disconnesso.");
}
