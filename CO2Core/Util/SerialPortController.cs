using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace CO2Core.Util
{
    public class SerialPortController
    {
        private SerialPort serialPort = null;
        public bool PortOpen(string portNum)
        {
            if (serialPort == null)
                serialPort = new SerialPort
                {
                    PortName = portNum,                //ポート番号
                    BaudRate = 115200,                 //ボーレート
                    DataBits = 8,                      //データビット
                    Parity = Parity.None,              //パリティ
                    StopBits = StopBits.One,           //ストップビット
                    Handshake = Handshake.None,        //ハンドシェイク
                    Encoding = Encoding.UTF8,          //エンコード
                    WriteTimeout = 10000,              //書き込みタイムアウト[ms]
                    ReadTimeout = 10000,               //読み取りタイムアウト[ms]
                    NewLine = "\r\n"                   //改行コード指定
                };
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                }
                catch (Exception ex)
                {
                    Plugin.Log?.Error($"PortOpenError:{ex.Message}");
                    return false;
                }
            }
            return true;
        }
        public void PortClose()
        {
            serialPort?.Close();
            serialPort = null;
        }
        public void Send(string text)
        {
            if (serialPort == null) return;
            if (!serialPort.IsOpen) return;
            try
            {
                serialPort.WriteLine(text);
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error($"SendError:{ex.Message}");
            }
        }
        public string ReadData()
        {
            if (serialPort == null) return "PORT NULL";
            if (!serialPort.IsOpen) return "PORT CLOSE";
            var text = "";
            try
            {
                text = serialPort.ReadLine();
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error($"ReadError:{ex.Message}");
                text = "READ ERROR";
            }
            return text;
        }
        public static string[] GetPort()
        {
            return SerialPort.GetPortNames();
        }
    }
}
