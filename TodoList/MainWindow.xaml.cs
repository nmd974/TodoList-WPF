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
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace TodoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SQLiteConnection db = DBConnection.DBInit();
            Task task = new Task();
            SQLiteDataReader data = task.GetTasks(db);
            RenderScreen(data);
        }

        private void AddTask(object sender, RoutedEventArgs e)
        {
            SQLiteConnection db = DBConnection.DBInit();
            Task task = new Task();
            task.label = addNewTask.Text;
            task.ended = "0";
            task.archived = "0";
            Task.AddTask(db, task);
            addNewTask.Clear();
            AddTaskInput(task);
        }

        private void AddTaskInput(Task task)
        {
            TextBlock newBlock = new TextBlock();
            newBlock.Text = task.label;
            newBlock.Height = 40;
            newBlock.Margin = new Thickness(5);
            newBlock.Background = new SolidColorBrush(Colors.Gray);
            mainContent.Children.Add(newBlock);
        }

        private void RenderScreen(SQLiteDataReader data)
        {
            if (data.HasRows)
            {
                while (data.Read())
                {
                    Task newTask = new Task();
                    newTask.label = data.GetString(data.GetOrdinal("label"));
                    newTask.id = data.GetInt32(data.GetOrdinal("id"));
                    AddTaskInput(newTask);
                }
            }
        }
    }
}
