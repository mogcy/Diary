using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SQLite;

namespace Diary
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class AddDiaryWindow : Window
    {
        public AddDiaryWindow()
        {
            InitializeComponent();
        }

        private void AddDiaryToDb()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=DiaryDb.sqlite"))
            {
                // データベースに接続
                conn.Open();
                //日記一覧の表示
                using (DataSet dataSet = new DataSet())
                {
                    // データを挿入
                    if (this.textBoxForAddDiary.Text != "") {
                        String sql = String.Format("INSERT INTO diary (date , text) VALUES ('{0}', '{1}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.textBoxForAddDiary.Text);
                        SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(sql, conn);
                        dataAdapter.Fill(dataSet);
                    } else
                    {
                        MessageBox.Show("本文が入力されていません。", caption:"警告");
                    }
                }
                // 切断
                conn.Close();
            }
        }

        private void RsetBtnClick(object sender, RoutedEventArgs e)
        {
            this.textBoxForAddDiary.Text = "";
        }

        private void AddDiaryDataClick(object sender, RoutedEventArgs e)
        {
            AddDiaryToDb();
            this.Close();
        }
    }
}
