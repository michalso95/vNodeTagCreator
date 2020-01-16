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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;

namespace TagCreatorVNode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ReadWrite RWClass;
        List<string> tagList = new List<string>();
        readonly Regex regex = new Regex(@"^(-?[1-9]+\d*([.]\d+)?)$|^(-?0[.]\d*[1-9]+)$|^0$|^0.0$");
        readonly Regex regex1 = new Regex(@"^.?[CS]");
        readonly Regex regex2 = new Regex(@"^.?[IRP]");

        public MainWindow()
        {
            InitializeComponent();
            SetCombo();
            SetDefault();
            RWClass = new ReadWrite();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Multiselect = false
            };
            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName.Contains(".txt"))
                    RWClass.SetPath(ofd.FileName);
                else
                    MessageBox.Show("Error. Wrong file");
            }
            tagList = RWClass.tagList;
            counterLbl.Content = string.Format("Num of tags: {0}", RWClass.ammount);

            addBtn.IsEnabled = true;
            createBtn.IsEnabled = true;
        }

        private void SetCombo()
        {
            string[] typeArr = new string[] { "boolean", "string", "number" };
            string[] boolArr = new string[] { "true", "false" };
            string[] formatArr = new string[] { "%s", "%i" };
            string[] accessArr = new string[] { "R", "RW" };
            foreach (string obj in typeArr)
            {
                typeCombo.Items.Add(obj);

            }
            foreach (string obj in boolArr)
            {
                SimulationCombo.Items.Add(obj);
                ScalingCombo.Items.Add(obj);
                ScalingClamp1Combo.Items.Add(obj);
                ScalingClamp0Combo.Items.Add(obj);
                SourceEnabledCombo.Items.Add(obj);
                DiscardOldestCombo.Items.Add(obj);
                HistoryEnabledCombo.Items.Add(obj);
            }
            foreach (string obj in formatArr)
            {
                FormatCombo.Items.Add(obj); 
            }
            foreach (string obj in accessArr)
            {
                ClientAccessCombo.Items.Add(obj);
            }
        }

        private void SetDefault()
        {
            string defaultpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            outputLbl.Content = string.Format("{0}\\{1}.csv",defaultpath, "Tagfile");
            groupNameText.Text = "wago";
            TagNameText.Text = "";
            typeCombo.SelectedIndex = 0;
            FormatCombo.SelectedIndex = 0;
            deadbandText.Text = "0.1u";
            ClientAccessCombo.SelectedIndex = 1;
            engUnitsText.Text = "";
            DefaultText.Text = "<null>";
            SimulationCombo.SelectedIndex = 1;
            PersistencyText.Text = "0";
            ScalingCombo.SelectedIndex = 1;
            ScalingRaw0Text.Text = "0";
            ScalingRaw1Text.Text = "1000";
            ScalingEu0Text.Text = "0";
            ScalingEu1Text.Text = "1000";
            ScalingClamp0Combo.SelectedIndex = 1;
            ScalingClamp1Combo.SelectedIndex = 1;
            SourceEnabledCombo.SelectedIndex = 0;
            SourcetypeText.Text = "OpcUaClient";
            SourceModuleText.Text = "OPC_client";
            ConfigclientText.Text = "Connect";
            NodeIdText.Text = "ns=4;s=|var|WAGO 750-8212 PFC200 G2 2ETH RS.Application.IoConfig_Globals_Mapping.";
            SamplingIntervalText.Text = "100";
            DiscardOldestCombo.SelectedIndex = 0;
            QueueSizeText.Text = "5";
            HistoryEnabledCombo.SelectedIndex = 1;
            HistorymoduleText.Text = "";
        }
        private string GetDataType(string str)
        {
            string datatype = "";
            if (regex1.IsMatch(str))
                datatype = "boolean";
            else if (regex2.IsMatch(str))
                datatype = "number";
            else
                datatype = "string";

            return datatype;
        }

        private void MergeStrings(List<string> list)
        {
            string datatype;
            List<string> elements = new List<string>();
            string[] arrElem;
            for (int i = 0; i < list.Count() - 1; i++)
            {
                    arrElem = list[i].Split('.');
                    elements = arrElem.ToList<string>();
                    datatype = GetDataType(elements.Last());

                tagList[i] = string.Format("{0}/{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};\"{22}{1}\";{23};{24};{25};{26};{27}", 
                    groupNameText.Text, list[i], DescriptionText.Text, datatype, FormatCombo.SelectedItem.ToString(),
                    deadbandText.Text, ClientAccessCombo.SelectedItem.ToString(), engUnitsText.Text, DefaultText.Text, SimulationCombo.SelectedItem.ToString(),
                    PersistencyText.Text, ScalingCombo.SelectedItem.ToString(), ScalingRaw0Text.Text, ScalingRaw1Text.Text, ScalingEu0Text.Text,
                    ScalingEu1Text.Text, ScalingClamp0Combo.SelectedItem.ToString(), ScalingClamp1Combo.SelectedItem.ToString(), SourceEnabledCombo.SelectedItem.ToString(), SourcetypeText.Text,
                    SourceModuleText.Text, ConfigclientText.Text, NodeIdText.Text, SamplingIntervalText.Text, DiscardOldestCombo.SelectedItem.ToString(), 
                    QueueSizeText.Text, HistoryEnabledCombo.SelectedItem.ToString(), HistorymoduleText.Text);
            }
            tagList.Insert(0, ":Tag;description;type;format;deadband;clientAccess;engUnits;default;simulation.enabled;persistency;extensions.scaling.enabled;extensions.scaling.raw.0;extensions.scaling.raw.1;extensions.scaling.eu.0;extensions.scaling.eu.1;extensions.scaling.clamp.0;extensions.scaling.clamp.1;extensions.source.enabled;extensions.source.type;extensions.source.module;extensions.source.config.client;extensions.source.config.nodeId;extensions.source.config.options.samplingInterval;extensions.source.config.options.discardOldest;extensions.source.config.options.queueSize;extensions.history.enabled;extensions.history.module");
            tagList.Insert(0, "#Tag instances [extensions.source.config = OpcUaClient.TagSourceConfig]");
            tagList.Insert(0, "#version 2 ");

            tagList.Add("#Group instances");
            tagList.Add(":Group;description");
            tagList.Add(groupNameText.Text);
            tagList.Add("");
        }

        private string AddOneItem()
        {
            return string.Format("{0}/{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22}{1};{23};{24};{25};{26};{27}\n",
                    groupNameText.Text, TagNameText.Text, DescriptionText.Text, typeCombo.SelectedItem.ToString(), FormatCombo.SelectedItem.ToString(),
                    deadbandText.Text, ClientAccessCombo.SelectedItem.ToString(), engUnitsText.Text, DefaultText.Text, SimulationCombo.SelectedItem.ToString(),
                    PersistencyText.Text, ScalingCombo.SelectedItem.ToString(), ScalingRaw0Text.Text, ScalingRaw1Text.Text, ScalingEu0Text.Text,
                    ScalingEu1Text.Text, ScalingClamp0Combo.SelectedItem.ToString(), ScalingClamp1Combo.SelectedItem.ToString(), SourceEnabledCombo.SelectedItem.ToString(), SourcetypeText.Text,
                    SourceModuleText.Text, ConfigclientText.Text, NodeIdText.Text, SamplingIntervalText.Text, DiscardOldestCombo.SelectedItem.ToString(),
                    QueueSizeText.Text, HistoryEnabledCombo.SelectedItem.ToString(), HistorymoduleText.Text);
        }

        private void DefaultBtn_Click(object sender, RoutedEventArgs e) => SetDefault();

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RWClass.AddTag(AddOneItem(), outputLbl.Content.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err. Did you load tag file?\n" + ex.ToString());
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MergeStrings(tagList);
                RWClass.WriteCSV(tagList, outputLbl.Content.ToString());
                MessageBox.Show("CSV file has been created from tag file");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err. Did you load tag file?\n" + ex.ToString());
            }
        }

        private void BrowseOutput()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Change csv file location"
            };
            sfd.ShowDialog();
            outputLbl.Content = sfd.FileName;
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e) => BrowseOutput();
    }
}
