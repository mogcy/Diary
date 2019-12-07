using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Linq;

namespace Diary
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using (var conn = new SQLiteConnection("Data Source=DiaryDb.sqlite"))
            {
                // データベースに接続
                conn.Open();
                // コマンドの実行
                using (var command = conn.CreateCommand())
                {
                    // テーブルが存在しなければ作成する
                    // 日記テーブル
                    StringBuilder sb = new StringBuilder();
                    sb.Append("CREATE TABLE IF NOT EXISTS diary (");
                    sb.Append("text TEXT NOT NULL");
                    sb.Append("  , date TEXT NOT NULL");
                    sb.Append(")");

                    command.CommandText = sb.ToString();
                    command.ExecuteNonQuery();
                }
                // 切断
                conn.Close();
            }
        }

        /// 日記一覧をグリッドに表示
        private void ShowDiaryBtnClick(object sender, RoutedEventArgs e)
        {
            ShowDiaryToDataGrid();
        }
        public void ShowDiaryToDataGrid()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=DiaryDb.sqlite"))
            {
                // データベースに接続
                conn.Open();
                //日記一覧の表示
                using (DataSet dataSet = new DataSet())
                {
                    // データを取得
                    String sql = "SELECT date, text From diary ORDER BY date DESC LIMIT 1000";
                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(sql, conn);
                    dataAdapter.Fill(dataSet);
                    //データグリッドに表示
                    this.DiaryListViewGrid.AutoGenerateColumns = false; //自動で列を作成しない（列が重複して表示されるため）
                    this.DiaryListViewGrid.DataContext = dataSet.Tables[0].DefaultView;
                }
                // 切断
                conn.Close();
            }
        }
        private void AddDiaryBtnClick(object sender, RoutedEventArgs e)
        {
            AddDiaryWindow adw = new AddDiaryWindow();
            adw.Show();
        }
    }
}
