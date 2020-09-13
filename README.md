# MQTT_Monitoring_App
  ####  2020-09-10 UPDATE 사항 
+ DivisionModel.cs 가 추가되었습니다
![image](https://user-images.githubusercontent.com/60413311/92737024-c05a6080-f3b5-11ea-8be4-6f0dd921fe27.png)

+  HistoryView, HistoryViewModel 이추가되었습니다

![image](https://user-images.githubusercontent.com/60413311/92750097-b3dc0500-f3c1-11ea-9790-bc1d24178172.png)

``` C#
namespace MqttMonitoringApp.ViewModels
{
    class HistoryViewModel : Conductor<object>
    {
        
        private BindableCollection<DivisionModel> divisions;
        public BindableCollection<DivisionModel> Divisions
        {
            get => divisions;
            set
            {
                divisions = value;
                NotifyOfPropertyChange(() => Divisions);
            }
        }

        private IList<DataPoint> tempValues;
        public IList<DataPoint> TempValues
        {
            get => tempValues;
            set
            {
                tempValues = value;
                NotifyOfPropertyChange(() => TempValues);
            }
        }

        //HumidValues, PressValues
        private IList<DataPoint> humidValues;
        public IList<DataPoint> HumidValues
        {
            get => humidValues;
            set
            {
                humidValues = value;
                NotifyOfPropertyChange(() => HumidValues);
            }
        }

        private IList<DataPoint> pressValues;
        public IList<DataPoint> PressValues
        {
            get => pressValues;
            set
            {
                pressValues = value;
                NotifyOfPropertyChange(() => PressValues);
            }
        }

        private int totalCount;
        public int TotalCount
        {
            get => totalCount;
            set
            {
                totalCount = value;
                NotifyOfPropertyChange(() => TotalCount);
            }
        }

        public DivisionModel selectedDivision;
        public DivisionModel SelectedDivision
        {
            get => selectedDivision;
            set
            {
                selectedDivision = value;
                NotifyOfPropertyChange(() => SelectedDivision);
            }
        }

        private string startDate;
        public string StartDate
        {
            get => startDate;
            set
            {
                startDate = DateTime.Parse(value).ToString("yyyy-MM-dd");
                endDate = DateTime.Parse(startDate).AddDays(1).ToString("yyyy-MM-dd");
                NotifyOfPropertyChange(() => StartDate);
                NotifyOfPropertyChange(() => EndDate);
            }
        }

        private string endDate;
        public string EndDate
        {
            get => endDate;
            set
            {
                endDate = DateTime.Parse(value).ToString("yyyy-MM-dd");
                NotifyOfPropertyChange(() => EndDate);
            }
        }

        public HistoryViewModel()
        {
            InitControls();
            InitDataFromDB();
        }

        private void InitControls()
        {
            Divisions = new BindableCollection<DivisionModel>
            {
                new DivisionModel { KeyVal = 0, DivisionVal = "Select"},
                new DivisionModel { KeyVal = 1, DivisionVal = "DiningRoom"},
                new DivisionModel { KeyVal = 2, DivisionVal = "LivingRoom"},
                new DivisionModel { KeyVal = 3, DivisionVal = "BedRoom"},
                new DivisionModel { KeyVal = 4, DivisionVal = "BathRoom"},
                new DivisionModel { KeyVal = 5, DivisionVal = "GuestRoom"}
            };
            SelectedDivision = Divisions.Where(v => v.DivisionVal.Contains("Select")).FirstOrDefault();
        }

        private void InitDataFromDB()
        {
            Commons.CONNSTRING = "Server=localhost;Port=3306;" +
                "Database=iot_sensordata;Uid=root;Pwd=mysql_p@ssw0rd";


            using (var conn = new MySqlConnection(Commons.CONNSTRING))
            {
                string strSelQuery = "SELECT date_format(Curr_Time, '%Y-%m-%d') AS StartDate " +
                                     " FROM smarthometbl " +
                                     " WHERE Id = 1 ";

                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSelQuery, conn);
                    string result = cmd.ExecuteScalar().ToString();

                    StartDate = result;
                    EndDate = DateTime.Parse(result).AddDays(1).ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public void Search()
        {
            if (SelectedDivision.KeyVal == 0)
            {
                var wManger = new WindowManager();
                wManger.ShowDialog(new ErrorPopupViewModel("Error|Division을 선택하세요."));
                return;
            }

            TempValues = new List<DataPoint>();
            HumidValues = new List<DataPoint>();
            PressValues = new List<DataPoint>();
            List<DataPoint> listTemps = new List<DataPoint>();
            List<DataPoint> listHumids = new List<DataPoint>();
            List<DataPoint> listPresses = new List<DataPoint>();

            using (var conn = new MySqlConnection(Commons.CONNSTRING))
            {
                string strSelQuery = "SELECT Temp, Humid, Press " +
                                     "  FROM smarthometbl " +
                                     " WHERE Dev_Id = @Dev_Id  " +
                                     "   AND Curr_Time BETWEEN @StartDate AND @EndDate  " +
                                     " ORDER BY Id";

                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strSelQuery, conn);
                    MySqlParameter paramDevId = new MySqlParameter("@Dev_Id", MySqlDbType.VarChar);
                    paramDevId.Value = SelectedDivision.DivisionVal;
                    cmd.Parameters.Add(paramDevId);

                    MySqlParameter paramStartDate = new MySqlParameter("@StartDate", MySqlDbType.VarChar);
                    paramStartDate.Value = StartDate;
                    cmd.Parameters.Add(paramStartDate);

                    MySqlParameter paramEndDate = new MySqlParameter("@EndDate", MySqlDbType.VarChar);
                    paramEndDate.Value = EndDate;
                    cmd.Parameters.Add(paramEndDate);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    var i = 0;
                    while (reader.Read())
                    {
                        listTemps.Add(new DataPoint(i, Convert.ToDouble(reader["Temp"])));
                        listHumids.Add(new DataPoint(i, Convert.ToDouble(reader["Humid"])));
                        listPresses.Add(new DataPoint(i, Convert.ToDouble(reader["Press"])));

                        i++;
                    }

                    if (i == 0)
                    {
                        var wManger = new WindowManager();
                        wManger.ShowDialog(new ErrorPopupViewModel("Error|데이터가 없습니다."));
                        return;
                    }

                    TotalCount = i;
                }
                catch (Exception ex)
                {
                    var wManger = new WindowManager();
                    wManger.ShowDialog(new ErrorPopupViewModel($"Error|{ex.Message}"));
                    return;
                }

                TempValues = listTemps;
                HumidValues = listHumids;
                PressValues = listPresses;
            }
        }
    }
}
```

+ DataBaseViewModel의 InsertDataBase함수에 내용이 추가되었습니다.
``` C#
 private void InsertDataBase(string message)
        {
            var currDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            using (var conn = new MySqlConnection(Commons.CONNSTRING))
            {
                string strInsQuery = " INSERT INTO smarthometbl " +
                                     " ( " +
                                     "   Dev_Id, " +
                                     "   Curr_Time, " +
                                     "   Temp, " +
                                     "   Humid, " +
                                     "   Press " +
                                     " ) " +
                                     "  VALUES " +
                                     " ( " +
                                     "  @Dev_Id, " +
                                     "  @Curr_Time, " +
                                     "  @Temp, " +
                                     "  @Humid, " +
                                     "  @Press" +
                                     " ) ";

                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(strInsQuery, conn);
                    MySqlParameter paramDevId = new MySqlParameter("@Dev_Id", MySqlDbType.VarChar);
                    paramDevId.Value = currDatas["Dev_Id"];
                    cmd.Parameters.Add(paramDevId);

                    MySqlParameter paramCurrTime = new MySqlParameter("@Curr_Time", MySqlDbType.DateTime);
                    paramCurrTime.Value = DateTime.Parse(currDatas["Curr_Time"]);
                    cmd.Parameters.Add(paramCurrTime);

                    MySqlParameter paramTemp = new MySqlParameter("@Temp", MySqlDbType.Float);
                    paramTemp.Value = currDatas["Temp"];
                    cmd.Parameters.Add(paramTemp);

                    MySqlParameter paramHumid = new MySqlParameter("@Humid", MySqlDbType.Float);
                    paramHumid.Value = currDatas["Humid"];
                    cmd.Parameters.Add(paramHumid);

                    MySqlParameter paramPress = new MySqlParameter("@Press", MySqlDbType.Float);
                    paramPress.Value = currDatas["Press"];
                    cmd.Parameters.Add(paramPress);

                    if (cmd.ExecuteNonQuery() == 1)
                        UpdateText("[DB] Inserted");
                    else
                        UpdateText("[DB] Failed");
                }
                catch (Exception ex)
                {
                    UpdateText($">>> Message : {ex.Message}");
                }
            }
        }
```
