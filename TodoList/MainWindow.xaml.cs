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
        public Int32 lastId;
        public TextBlock Actual_TextBlock;
        public int taches_en_cours = 0;
        public int taches_terminees = 0;
        public MainWindow()
        {
            InitializeComponent();
            SQLiteConnection db = DBConnection.DBInit();
            Task task_access = new Task();
            SQLiteDataReader data = task_access.GetTasks(db);
            RenderScreen(data);
        }

        private void AddTask()
        {
            SQLiteConnection db = DBConnection.DBInit();
            Task task = new Task();
            if(addNewTask.Text != "")
            {
                task.label = addNewTask.Text.Trim();
                task.ended = 0;
                task.archived = 0;
                task.id = this.lastId + 1;
                task.AddTask(db, task);
                addNewTask.Clear();
                AddTaskInput(task);
                this.lastId = Int32.Parse(task.id.ToString());
                taches_en_cours += 1;
                RerenderScreen();
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftCtrl:
                    addNewTask.Text = "            ";
                    addNewTask.Focus();
                    
                    break;
                case Key.Enter:
                    AddTask();
                    break;

            }
        }
        private void AddTaskInput(Task task)
        {
            TextBlock newBlock = new TextBlock();
            newBlock.Text = task.label;
            newBlock.Uid = task.id.ToString();
            newBlock.Style = (Style)FindResource("inputTask");
            newBlock.MouseRightButtonDown += new MouseButtonEventHandler(SelectedInput);
            if (task.ended != 0)
            {
                newBlock.ContextMenu = (ContextMenu)FindResource("contextMenuEnded");
                Taches_termines.Children.Add(newBlock);
                taches_terminees += 1;
            }
            else
            {
                newBlock.ContextMenu = (ContextMenu)FindResource("contextMenu");
                Taches_en_cours.Children.Add(newBlock);
                taches_en_cours += 1;
            }           
        }
        private void SelectedInput(object sender, MouseButtonEventArgs e)
        {
            this.Actual_TextBlock = (TextBlock)sender;
        }
        private void DeleteTaskInput(object sender, EventArgs e)
        {
            SQLiteConnection db = DBConnection.DBInit();
            TextBlock taskPressed = this.Actual_TextBlock;
            StackPanel parent_element = (StackPanel)taskPressed.Parent;
            parent_element.Children.Remove(taskPressed);
            Task task_access = new Task();
            
            if (parent_element.Name == "Taches_en_cours")
            {
                taskPressed.ContextMenu = (ContextMenu)FindResource("contextMenuEnded");
                parent_element.Children.Remove(taskPressed);
                taches_en_cours -= 1;
            }
            else
            {
                taskPressed.ContextMenu = (ContextMenu)FindResource("contextMenu");
                parent_element.Children.Remove(taskPressed);
                taches_terminees -= 1;
            }
            task_access.ArchiveTask(db, Int32.Parse(taskPressed.Uid));
            RerenderScreen();
        }

        private void EndedTaskInput(object sender, EventArgs e)
        {
            Task task_access = new Task();
            SQLiteConnection db = DBConnection.DBInit();
            TextBlock taskPressed = this.Actual_TextBlock;
            StackPanel parent_element = (StackPanel)taskPressed.Parent;

            if(parent_element.Name == "Taches_en_cours")
            {
                taskPressed.ContextMenu = (ContextMenu)FindResource("contextMenuEnded");
                parent_element.Children.Remove(taskPressed);
                Taches_termines.Children.Add(taskPressed);
                
                task_access.EndTask(db, Int32.Parse(taskPressed.Uid));
                AddToEnded();
            }
            else
            {
                taskPressed.ContextMenu = (ContextMenu)FindResource("contextMenu");
                parent_element.Children.Remove(taskPressed);
                Taches_en_cours.Children.Add(taskPressed);
                
                task_access.ReactivateTask(db, Int32.Parse(taskPressed.Uid));
                AddToInProgress();
            }
        }

        private void RenderScreen(SQLiteDataReader data)
        {
            string date = DateTime.Now.ToString("dddd dd MMMM yyyy");
            Date.Text = date.First().ToString().ToUpper() + date.Substring(1);
            Debug.WriteLine(data);
            if (data.HasRows)
            {
                while (data.Read())
                {
                    Task newTask = new Task();
                    newTask.label = data.GetString(data.GetOrdinal("label"));
                    newTask.id = data.GetInt32(data.GetOrdinal("id"));
                    newTask.ended = data.GetInt32(data.GetOrdinal("ended"));

                    AddTaskInput(newTask);
                    this.lastId = Int32.Parse(newTask.id.ToString());
                    Expander_Terminees.Header = "Tâches terminées (" + taches_en_cours + ")";
                    Expander_En_Cours.Header = "Tâches en cours (" + taches_terminees + ")";
                }
            }
            else
            {
                Expander_Terminees.Header = "Tâches terminées (" + taches_en_cours + ")";
                Expander_En_Cours.Header = "Tâches en cours (" + taches_terminees + ")";
            }
        }

        private void AddToEnded()
        {
            if (taches_en_cours > 0)
            {
                taches_en_cours -= 1;
            }
            else
            {
                taches_en_cours = 0;
            }

            taches_terminees += 1;
            RerenderScreen();
        }

        private void AddToInProgress()
        {
            taches_en_cours += 1;

            if (taches_terminees > 0)
            {
                taches_terminees -= 1;
            }
            else
            {
                taches_terminees = 0;
            }
            RerenderScreen();
        }

        private void RerenderScreen()
        {
            Expander_Terminees.Header = "Tâches terminées (" + taches_terminees + ")";
            Expander_En_Cours.Header = "Tâches en cours (" + taches_en_cours + ")"; 
        }
    }
}
