using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using System.Diagnostics;

namespace OLypiadSorting
{
    public partial class SortingApp : Form
    {
        int[] NumberArray;
        int[] IterationArray = new int[5];
        long[] TimerArray = new long[5];
        bool ascendingKey = true;
        private string fileName = string.Empty;
        private DataTableCollection tableCollection = null;

    private void SetAscendingKey()
    {
      ascendingKey = ascendingBox.Checked;
    }

        private void GetArray() // получение чисел из gridViev в NumberArray
        {
            try
            {
                NumberArray = new int[dataGridView1.Rows.Count];
                for (int index = 0; index < dataGridView1.Rows.Count - 1; ++index)
                {
                    NumberArray[index] = Convert.ToInt32(dataGridView1[0, index + 1].Value);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось преобразовать данные из Excel", "Ошибка конвертации!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BubbleSort(int[] array, bool ascending) // пузырьковая сортировка
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                ++IterationArray[0];
                if ((ascending && array[j] > array[j + 1]) || (!ascending && array[j] < array[j + 1]))
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }

    private void BubbleSortRealize()
    {
      int[] BubbleArray = NumberArray;
      Stopwatch stopwatch = new Stopwatch();
      IterationArray[0] = 0;
      TimerArray[0] = 0;

      stopwatch.Start();
      BubbleSort(BubbleArray, ascendingKey);
      stopwatch.Stop();

      TimerArray[0] = stopwatch.ElapsedTicks;
      chart1.Series[0].Points.Clear();
      for(int index = 0; index < NumberArray.Length; ++index)
      {
        chart1.Series[0].Points.AddXY(index, BubbleArray[index]);
      }

      bubbleIterations.Text = Convert.ToString(IterationArray[0]);
      bubbleTime.Text = Convert.ToString(TimerArray[0]);
    }

        private void InsertionSort(int[] array, bool ascending) // сортировка вставками
        {
            int n = array.Length;

            for (int i = 1; i < n; i++)
            {
                int key = array[i];
                int j = i - 1;

        ++IterationArray[1];
        // Перемещаем элементы большие (или меньшие, в зависимости от направления) чем key, на одну позицию вперед.
        while (j >= 0 && ((ascending && array[j] > key) || (!ascending && array[j] < key)))
                {
                    array[j + 1] = array[j];
                    j = j - 1;
                }

                array[j + 1] = key;
            }
        }

    private void InsertionSortRealize()
    {
      int[] InsertionArray = NumberArray;
      Stopwatch stopwatch = new Stopwatch();
      IterationArray[1] = 0;
      TimerArray[1] = 0;

      stopwatch.Start();
      InsertionSort(InsertionArray, ascendingKey);
      stopwatch.Stop();

      TimerArray[1] = stopwatch.ElapsedTicks;
      chart1.Series[1].Points.Clear();
      for (int index = 0; index < NumberArray.Length; ++index)
      {
        chart1.Series[1].Points.AddXY(index, InsertionArray[index]);
      }

      insertIterations.Text = Convert.ToString(IterationArray[1]);
      insertTime.Text = Convert.ToString(TimerArray[1]);
    }

    private void ShakerSort(int[] array, bool ascending) // шейкерная сортировка
        {
            int left = 0;
            int right = array.Length - 1;
            bool swapped;

            do
            {
                swapped = false;

                // Проход справа налево, сдвигая наибольший элемент вправо.
                for (int i = left; i < right; i++)
                {
          ++IterationArray[2];
          if ((ascending && array[i] > array[i + 1]) || (!ascending && array[i] < array[i + 1]))
                    {
                        // Обмен элементов
                        int temp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = temp;
                        swapped = true;
                    }
                }

                if (!swapped)
                    break;

                swapped = false;
                right--;

                // Проход слева направо, сдвигая наименьший элемент влево.
                for (int i = right; i > left; i--)
                {
          ++IterationArray[2];
          if ((ascending && array[i] < array[i - 1]) || (!ascending && array[i] > array[i - 1]))
                    {
                        // Обмен элементов
                        int temp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp;
                        swapped = true;
                    }
                }

                left++;
            } while (swapped);
        }

    private void ShakerSortRealize()
    {
      int[] InsertionArray = NumberArray;
      Stopwatch stopwatch = new Stopwatch();
      IterationArray[2] = 0;
      TimerArray[2] = 0;

      stopwatch.Start();
      ShakerSort(InsertionArray, ascendingKey);
      stopwatch.Stop();

      TimerArray[2] = stopwatch.ElapsedTicks;
      chart1.Series[2].Points.Clear();
      for (int index = 0; index < NumberArray.Length; ++index)
      {
        chart1.Series[2].Points.AddXY(index, InsertionArray[index]);
      }

      shakerIterations.Text = Convert.ToString(IterationArray[2]);
      shakerTime.Text = Convert.ToString(TimerArray[2]);
    }

    private void QuickSort(int[] array, int left, int right, bool ascending) // быстрая сортировка
        {
            if (left < right)
            {
        int partitionIndex = Partition(array, left, right, ascending);
        // Рекурсивно сортируем элементы до и после опорного элемента
        QuickSort(array, left, partitionIndex - 1, ascending);
                QuickSort(array, partitionIndex + 1, right, ascending);
            }
    }

        private int Partition(int[] array, int left, int right, bool ascending)
        {
            int pivot = array[right];
            int i = (left - 1);

            for (int j = left; j < right; j++)
            {
        ++IterationArray[3];
        if ((ascending && array[j] <= pivot) || (!ascending && array[j] >= pivot))
                {
                    i++;

                    // Обмен элементов
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }

            // Обмен опорного элемента с элементом, находящимся на позиции i+1
            int swap = array[i + 1];
            array[i + 1] = array[right];
            array[right] = swap;

            return i + 1;
        }

    private void QuickSortRealize()
    {
      int[] QuickArray = NumberArray;
      Stopwatch stopwatch = new Stopwatch();
      IterationArray[3] = 0;
      TimerArray[3] = 0;

      stopwatch.Start();
      QuickSort(QuickArray, 0 , QuickArray.Length - 1, ascendingKey);
      stopwatch.Stop();

      TimerArray[3] = stopwatch.ElapsedTicks;
      chart1.Series[3].Points.Clear();
      for (int index = 0; index < NumberArray.Length; ++index)
      {
        chart1.Series[3].Points.AddXY(index, QuickArray[index]);
      }

      quickIterations.Text = Convert.ToString(IterationArray[3]);
      quickTime.Text = Convert.ToString(TimerArray[3]);
    }

    private static void Shuffle(int[] data)
    {
      int temp, rnd;
      Random rand = new Random();

      for (int i = 0; i < data.Length; ++i)
      {
        rnd = rand.Next(data.Length);
        temp = data[i];
        data[i] = data[rnd];
        data[rnd] = temp;
      }
    }


        private int[] BogoSort(int[] data, bool ascending)
        {
            while (!IsSorted(data, ascending))
            {
                ++IterationArray[4];

        if (IterationArray[4] > 500000)
        {
          MessageBox.Show("Количество итераций превысило 500000", "Внимание");
          break;
        }

        Shuffle(data);
            }

            return data;
        }

        private static bool IsSorted(int[] data, bool ascending)  //bogo 
    {
            int count = data.Length;

            for (int i = 1; i < count; i++)
            {
                if (ascending)
                {
                    if (data[i] < data[i - 1])
                    {
                        return false;
                    }
                }
                else
                {
                    if (data[i] > data[i - 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        } 

    private void BogoSortRealize()
    {
      int[] BogoArray = NumberArray;
      Stopwatch stopwatch = new Stopwatch();
      IterationArray[4] = 0;
      TimerArray[4] = 0;

      stopwatch.Start();
      BogoSort(BogoArray, ascendingKey);
      stopwatch.Stop();

      TimerArray[4] = stopwatch.ElapsedTicks;
      chart1.Series[4].Points.Clear();

      if (IterationArray[4] != 500001)
      {
        for (int index = 0; index < NumberArray.Length; ++index)
        {
          chart1.Series[4].Points.AddXY(index, BogoArray[index]);
        }
      }

      bogoIterations.Text = Convert.ToString(IterationArray[4]);
      bogoTime.Text = Convert.ToString(TimerArray[4]);
    }

    private void RandomGerenation()
    {
      if (dataGridView1.ColumnCount > 0)
      {
        dataGridView1.Columns.Clear();
      }

      int ArraySize = 0;
      int MaxNumber = 1;
      int MinNumber = 0;

            try
            {
              ArraySize = Convert.ToInt32(generationCountBox.Text);
              MaxNumber = Convert.ToInt32(maxValueBox.Text);
              MinNumber = Convert.ToInt32(minValueBox.Text);
            }
            catch
            {
                MessageBox.Show("Неправильно введены данные для генерации списка чисел.", "Ошибка конвертации!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
        if(MaxNumber < MinNumber)
        {
          MessageBox.Show("Минимальное число не может быть больше максимального.", "Неправильные входные данные!", MessageBoxButtons.OK, MessageBoxIcon.Error);
          MaxNumber = 1;
          MinNumber = 0;
        }
        NumberArray = new int[ArraySize];
      }
      Random r = new Random();
      for(int index = 0; index < ArraySize; ++index)
      {
        NumberArray[index] = r.Next(MinNumber, MaxNumber);
      }
    }

        public SortingApp()
        {
            InitializeComponent();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAscendingKey();

      try
      {
        GetArray();
        if (dataGridView1.ColumnCount <= 0)
        {
          MessageBox.Show("Нет данных для обработки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
          if (BubbleBox.Checked)
          {
            GetArray();
            BubbleSortRealize();
          }
          else
          {
            bubbleTime.Text = "_______";
            bubbleIterations.Text = "_______";
          }
          if (InsertBox.Checked)
          {
            GetArray();
            InsertionSortRealize();
          }
          else
          {
            insertTime.Text = "_______";
            insertIterations.Text = "_______";
          }
          if (ShakerBox.Checked)
          {
            GetArray();
            ShakerSortRealize();
          }
          else
          {
            shakerTime.Text = "_______";
            shakerIterations.Text = "_______";
          }
          if (qickSort.Checked)
          {
            GetArray();
            QuickSortRealize();
          }
          else
          {
            quickTime.Text = "_______";
            quickIterations.Text = "_______";
          }
          if (BOGOSort.Checked)
          {
            GetArray();
            BogoSortRealize();
          }
          else
          {
            bogoTime.Text = "_______";
            bogoIterations.Text = "_______";
          }
        }

            }
            catch
            {
                MessageBox.Show("Создайте массив", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumberArray = null;
            for (int index = 0; index < 5; ++index)
            {
              chart1.Series[index].Points.Clear();
            }
            bubbleTime.Text = "_______";
            bubbleIterations.Text = "_______";
            insertTime.Text = "_______";
            insertIterations.Text = "_______";
            shakerTime.Text = "_______";
            shakerIterations.Text = "_______";
            quickTime.Text = "_______";
            quickIterations.Text = "_______";
            bogoTime.Text = "_______";
            bogoIterations.Text = "_______";
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
        if (dataGridView1.ColumnCount > 0)
        {
          dataGridView1.Columns.Clear();
        }

        DialogResult res = openFileDialog1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                    Text = fileName;
                    OpenExcelFile(fileName);
                }
                else
                {
                    throw new Exception("Файл не выбран!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

      GetArray();
      GenerateToolStripMenuItem.Enabled = false;
    }
        private void OpenExcelFile(string path)
        {
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
            DataSet db = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (x) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            tableCollection = db.Tables;
            
            toolStripComboBox1.Items.Clear();
            foreach (DataTable tebe in tableCollection)
            {
                toolStripComboBox1.Items.Add(tebe.TableName);
            }

            toolStripComboBox1.SelectedIndex = 0;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable table = tableCollection[Convert.ToString(toolStripComboBox1.SelectedItem)];
            dataGridView1.DataSource = table;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SortingApp_Load(object sender, EventArgs e)
        {

        }

        private void SortingApp_Load_1(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            RandomGerenation();

            int m = NumberArray.Length;


            dataGridView1.ColumnCount = 1; // Установите количество столбцов заранее

            for (int i = 0; i < m; ++i)
            {
              dataGridView1.Rows.Add(); // Добавьте новую строку
              dataGridView1.Rows[i].Cells[0].Value = NumberArray[i]; // Заполните значение в первом столбце
            }
        }
    }
}
