using ExcelConverter.Utils;
using ExcelToCode.Excel;

namespace ExcelToCode
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<string> fileList = null;
        private void OnFormLoaded(object sender, EventArgs e)
        {
            this.CenterToScreen();
            LogUtil.Init(this.logTb, this.errLogTb);
            //��ʼ��������Ϣ
            if (!Setting.Init())
            {
                MessageBox.Show("�Ҳ������������ļ�");
                return;
            }
            //��ȡ���ñ�·�����Ͳ���ʾ
            fileList = FileUtil.GetFileList(Setting.ConfigPath, false, ".xlsx");
            this.configListBox.Items.AddRange(fileList.ToArray());
        }

        private void ServerAll_Click(object sender, EventArgs e)
        {
            ExportAll(ExportType.Server);
        }

        private void ServerSingle_Click(object sender, EventArgs e)
        {
            ExportSingle(ExportType.Server);
        }

        private void ClientAll_Click(object sender, EventArgs e)
        {
            ExportAll(ExportType.Client);
        }

        private void ClientSingle_Click(object sender, EventArgs e)
        {
            ExportSingle(ExportType.Client);
        }


        private void ClearLogBtn_Click(object sender, EventArgs e)
        {
            this.logTb.Text = "";
        }

        private void ClearErrLogBtn_Click(object sender, EventArgs e)
        {
            this.errLogTb.Text = "";
        }

        private void ExportAll(ExportType etype)
        {
            if (fileList == null || fileList.Count <= 0)
            {
                MessageBox.Show("û�з������ñ�");
                return;
            }
            ToggleAllBtn(false);
            var startTime = TimeUtils.CurrentTimeMillis();
            ExportHelper.Export(etype, fileList, true);
            LogUtil.Add("�����ʱ��" + (TimeUtils.CurrentTimeMillis() - startTime) + "ms");
        }

        private void ExportSingle(ExportType etype)
        {
            var items = this.configListBox.CheckedItems;
            var selectList = new List<string>();
            foreach (var o in items)
            {
                var str = this.configListBox.GetItemText(o);
                if (!selectList.Contains(str))
                    selectList.Add(str);
            }
            if (selectList == null || selectList.Count <= 0)
            {
                MessageBox.Show("����ѡ��һ�����ñ�");
                return;
            }
            if (selectList == null || selectList.Count > 1)
            {
                MessageBox.Show("��֧�ֵ�ѡ");
                return;
            }
            ToggleAllBtn(false);
            ExportHelper.Export(etype, selectList, false);
        }

        public void ToggleAllBtn(bool flag)
        {
            ServerAll.Enabled = flag;
            ServerSingle.Enabled = flag;
            ClientAll.Enabled = flag;
            ClientSingle.Enabled = flag;
        }
    }
}