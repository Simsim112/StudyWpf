using Caliburn.Micro;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using WpfSmartHomeMonitoringApp.Helpers;
using WpfSmartHomeMonitoringApp.Models;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    class HistoryViewModel : Screen
    {
        private BindableCollection<DivisionModel> divisions;
        private DivisionModel selectedDivision;
        private string startDate;
        private string initStartDate;
        private string endDate;
        private string initEndDate;
        private int totalCount;
        private PlotModel historyModel; //oxyplot 220613, LJT, smartHomeModel -> historyModel

        /*
            Divisions
            DivisionVal
            SelectedDivision
            StartDate
            InitStartDate
            EndDate
            InitEndDate
            TotalCount
            SearchIoTData()
            SmartHomeModel*/

        public BindableCollection<DivisionModel> Divisions
        {
            get => divisions; 
            set
            {
                divisions = value;
                NotifyOfPropertyChange(() => Divisions);
            }
        }
        public DivisionModel SelectedDivision
        {
            get => selectedDivision; 
            set
            {
                selectedDivision = value;
                NotifyOfPropertyChange(() => SelectedDivision);
            }
        }
        public string StartDate
        {
            get => startDate; 
            set
            {
                startDate = value;
                NotifyOfPropertyChange(() => StartDate);
            }
        }
        public string InitStartDate
        {
            get => initStartDate; 
            set
            {
                initStartDate = value;
                NotifyOfPropertyChange(() => InitStartDate);
            }
        }
        public string EndDate
        {
            get => endDate; 
            set
            {
                endDate = value;
                NotifyOfPropertyChange(() => EndDate);
            }
        }
        public string InitEndDate
        {
            get => initEndDate; 
            set
            {
                initEndDate = value;
                NotifyOfPropertyChange(() => InitEndDate);
            }
        }
        public int TotalCount
        {
            get => totalCount; 
            set
            {
                totalCount = value;
                NotifyOfPropertyChange(() => TotalCount);
            }
        }
        public PlotModel HistoryModel
        {
            get => historyModel; 
            set
            {
                historyModel = value;
                NotifyOfPropertyChange(() => HistoryModel);
            }
        }

        public HistoryViewModel()
        {
            Commons.CONNSTRING = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";
            InitControl();
        }

        private void InitControl()
        {
            Divisions = new BindableCollection<DivisionModel> //콤보박스용 데이터 생성
            { 
                new DivisionModel { KeyVal = 0, DivisionVal = "-- Select --"},
                new DivisionModel { KeyVal = 1, DivisionVal = "DINING"},
                new DivisionModel { KeyVal = 2, DivisionVal = "LIVING"},
                new DivisionModel { KeyVal = 3, DivisionVal = "BED"},
                new DivisionModel { KeyVal = 4, DivisionVal = "BATH"}
            };
            //Select를 선택해서 초기화
            SelectedDivision = Divisions.Where(v => v.DivisionVal.Contains("Select")).FirstOrDefault();

            InitStartDate = DateTime.Now.ToShortDateString(); // 2022-06-09
            InitEndDate = DateTime.Now.AddDays(1).ToShortDateString(); // 2022-06-11

        }

        //검색 메서드
        public void SearchIoTData() 
        { 

            //Validation check
            if(SelectedDivision.KeyVal == 0) //Select
            {
                MessageBox.Show("검색할 방을 선택하세요");
                return;
            }

            if(DateTime.Parse(StartDate) > DateTime.Parse(endDate))
            {
                MessageBox.Show("시작일이 종료일보다 최신일 수 없습니다.");
                return;
            }

            TotalCount = 0;

            using(SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                string strQuery = @"SELECT Id, CurrTime, Temp, Humid 
                                        FROM TblSmartHome
                                        WHERE DevId = @DevId
                                        AND CurrTime BETWEEN @StartDate AND @EndDate
                                        ORDER BY Id Asc";

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(strQuery, conn);

                    SqlParameter parmDevId = new SqlParameter(@"DevId", SelectedDivision.DivisionVal);
                    cmd.Parameters.Add(parmDevId);

                    SqlParameter parmStartDate = new SqlParameter(@"StartDate", StartDate);
                    cmd.Parameters.Add(parmStartDate);

                    SqlParameter parmEndDate = new SqlParameter(@"EndDate", EndDate);
                    cmd.Parameters.Add(parmEndDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    var i = 0;
                    // start of 차트처리 작성 220613 추가
                    // 임시 플롯모델 
                    PlotModel temp = new PlotModel()
                    {
                        Title = $"{SelectedDivision.DivisionVal} Histories",
                        Subtitle = "using OxyPlot"
                    };

                    // 범례추가
                    var l = new Legend
                    {
                        LegendBorder = OxyColors.Black,
                        LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                        LegendPosition = LegendPosition.RightTop,
                        LegendPlacement = LegendPlacement.Inside

                    };

                    //온도값을 LineChart로 담을 객체
                    LineSeries seriesTemp = new LineSeries()
                    {
                        Color = OxyColor.FromRgb(255, 100, 100),
                        Title = "Temperature",
                        MarkerType = MarkerType.Circle,
                        MarkerSize = 4
                    };

                    // 습도값을 LineChart로 담을 객체
                    LineSeries seriesHumid = new LineSeries()
                    {
                        Color = OxyColor.FromRgb(150, 150, 255),
                        Title = "Humidity",
                        MarkerType = MarkerType.Triangle,
                        MarkerSize = 3
                    };

                    while (reader.Read())
                    {
                        //var Tmep = reader["Temp"];
                        // Temp,Humid 차트데이터 생성
                        seriesTemp.Points.Add(new DataPoint(i, Convert.ToDouble(reader["Temp"])));
                        seriesHumid.Points.Add(new DataPoint(i, Convert.ToDouble(reader["Humid"])));


                        i++;
                    }

                    TotalCount = i; // 검색한 데이터 촉 개수

                    temp.Series.Add(seriesTemp);
                    temp.Series.Add(seriesHumid);
                    temp.Legends.Add(l);
                    HistoryModel = temp;
                }

                // end of 차트처리 작성 220613 추가
                catch (Exception ex)
                {
                    MessageBox.Show($"Error {ex.Message}");
                    return;
                }
            }
        }
    }
}
