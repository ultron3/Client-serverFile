// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Net.Sockets;

public class Program
{
    public static void Main(string[]args)
    {
        string serverIp = "192.168.1.114"; // Indirizzo IP del server
        int serverPort = 5550;

        using (TcpClient client = new TcpClient(serverIp, serverPort))
        {
            using (NetworkStream networkStream = client.GetStream())
            {
                Console.Write("Inserisci il percorso completo del file exe da inviare: ");
                string filePath ="program.exe";
                byte[] fileData = File.ReadAllBytes(filePath);
                networkStream.Write(fileData, 0, fileData.Length);
            }
        }

        Console.WriteLine("File inviato al server.");
    }
}
