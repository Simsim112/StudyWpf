using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfNaverNewsSearch
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Commons.ShowMessageAsync("실행", "뉴스검색 실행");
                SearchNaverNews();
            }
        }

        private void SearchNaverNews()
        {
            string clientID = "V4Nda6C8IBBBRmXax7Ah";
            string clientSecret = "COp3LVn5c0";
            string keyword = txtSearch.Text;
            string openApiUri = $"https://openapi.naver.com/v1/search/news.json?start=1&display=100&query={keyword}";
            string result = string.Empty;

            WebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;

            //Naver OpenAPI 실제 요청
            try
            {
                request = WebRequest.Create(openApiUri); //URI에 대한 요청 만듬
                request.Headers.Add("X-Naver-Client-Id", clientID); //헤더에 아이디랑
                request.Headers.Add("X-Naver-Client-Secret", clientSecret); //비밀번호 넣음

                response = request.GetResponse(); //인터넷 요청에 대한 응답을 반환
                stream = response.GetResponseStream(); //사용되는 스트림을 가져옵니다.
                reader = new StreamReader(stream);

                result = reader.ReadToEnd(); // 현재 위치부터 스트림 끝까지의 모든 문자를 읽습니다.

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                reader.Close();
                stream.Close();
                response.Close();
            }
            MessageBox.Show(result);
            //var parsedJson = JObject.Parse(result); //string to json
        }
    }
}
