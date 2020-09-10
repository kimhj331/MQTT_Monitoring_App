using Bogus;
using MetroFramework.Forms;
using Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;

namespace BogusMqttPublishApp
{
    public partial class MainForm : MetroForm
    {
        public string MqttBrokerUrl { get; private set; }
        public MqttClient BrokerClient { get; set; }
        private Thread MqttThread { get; set; }
        private Faker<SensorInfo> SensorFaker { get; set; }
        private string CurrValue { get; set; }

        public BackgroundWorker MqttWorker { get; set; }

        private void InitializeAll()
        {
            MqttWorker = new BackgroundWorker();

            MqttWorker.DoWork += MqttWorker_DoWork;
            MqttWorker.WorkerReportsProgress = false;
            MqttWorker.WorkerSupportsCancellation = true;

            MqttBrokerUrl = "localhost"; // 또는 127.0.0.1 /자신의 IP

            string[] Rooms = new[] { "DiningRoom", "LivingRoom", "BathRoom", "BedRoom" };

            SensorFaker = new Faker<SensorInfo>()
               .RuleFor(s => s.Dev_Id, f => f.PickRandom(Rooms))
               .RuleFor(s => s.Curr_Time, f => f.Date.Past(0).ToString("yyyy-MM-dd HH:mm:ss.ff"))
               .RuleFor(s => s.Temp, f => float.Parse(f.Random.Float(19.0f, 35.9f).ToString("0.00")))
               .RuleFor(s => s.Humid, f => float.Parse(f.Random.Float(40.0f, 63.9f).ToString("0.0")))
               .RuleFor(s => s.Press, f => float.Parse(f.Random.Float(800.0f, 1000.0f).ToString("0.0")));
        }

        private void MqttWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoopPublish();
        }

        public MainForm()
        {
            InitializeComponent();
            InitializeAll();
            TxtBrokerIp.Text = "localhost";
        }

        private void BtnConn_Click(object sender, EventArgs e)
        {
            ConnectMqttBroker();
            // StartPublish();
            MqttWorker.RunWorkerAsync();
        }

        private void StartPublish()
        {
            MqttThread = new Thread(new ThreadStart(LoopPublish));
            MqttThread.Start();
        }

        private void ConnectMqttBroker()
        {
            MqttBrokerUrl = TxtBrokerIp.Text;
            BrokerClient = new MqttClient(MqttBrokerUrl);
            BrokerClient.Connect("FakerDaemon");
        }

        private void LoopPublish()
        {
            while (true)
            {
                SensorInfo value = SensorFaker.Generate();
                CurrValue = JsonConvert.SerializeObject(value, Formatting.Indented);
                BrokerClient.Publish("home/device/data/", Encoding.Default.GetBytes(CurrValue));
                //WriteLine($"Published : {CurrValue}");
                this.Invoke(new Action(() =>
                {
                    RtbLog.AppendText($"\n\nPublished : {CurrValue}");
                    RtbLog.ScrollToCaret();
                }));
                Thread.Sleep(1000);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
