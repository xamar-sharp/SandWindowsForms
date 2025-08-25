using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace SandWindowsForms
{
    public partial class Form1 : Form
    {
        [Browsable(true)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public BindingList<DataUnitViewModel> Units { get; set; }
        [Browsable(true)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public ICommand RandomContentCommand { get; set; }
        [Browsable(true)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public ICommand ChangeNameCommand { get; set; }
        public Form1()
        {
            InitializeComponent();

            Units = new BindingList<DataUnitViewModel>(new List<DataUnitViewModel>(8)
    {
        new DataUnitViewModel(new DataUnitModel() { Content = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, CreatedAt = DateTime.MaxValue, Id = Guid.NewGuid(), Name = "First" }),
        new DataUnitViewModel(new DataUnitModel() { Content = new byte[] { 1, 2, 3, 4, 5, 6, 7 }, CreatedAt = DateTime.Now.AddDays(-1), Id = Guid.NewGuid(), Name = "Second" }),
        new DataUnitViewModel(new DataUnitModel() { Content = new byte[] { 1, 2, 3, 4, 5, 6 }, CreatedAt = DateTime.Now.AddDays(-2), Id = Guid.NewGuid(), Name = "Third" }),
        new DataUnitViewModel(new DataUnitModel() { Content = new byte[] { 1, 2, 3, 4, 5 }, CreatedAt = DateTime.Now.AddDays(-3), Id = Guid.NewGuid(), Name = "Fourth" }),
        new DataUnitViewModel(new DataUnitModel() { Content = new byte[] { 1, 2, 3, 4 }, CreatedAt = DateTime.Now.AddDays(-4), Id = Guid.NewGuid(), Name = "Fivth" }),
        new DataUnitViewModel(new DataUnitModel() { Content = new byte[] { 1, 2, 3 }, CreatedAt = DateTime.Now.AddDays(-5), Id = Guid.NewGuid(), Name = "Sixth" }),
        new DataUnitViewModel(new DataUnitModel() { Content = new byte[] { 1, 2 }, CreatedAt = DateTime.Now.AddDays(-6), Id = Guid.NewGuid(), Name = "Seventh" }),
        new DataUnitViewModel(new DataUnitModel() { Content = new byte[] { 1 }, CreatedAt = DateTime.Now, Id = Guid.NewGuid(), Name = "Eight" })
    });

            TextBox forNameChanging = new TextBox() { Size = new Size(100, 45), Location = new Point(10, 10), Dock = DockStyle.Fill };
            ListBox listBox = new ListBox() { Size = new Size(400, 350), Location = new Point(150, 50) };
            Button randomByteGen = new Button() { Size = new Size(200, 85), Location = new Point(250, 420), Text = "Random Byte Gen" };
            Button changeName = new Button() { Size = new Size(200, 85), Location = new Point(250, 510), Text = "Change Name" };
            ListBox contentBox = new ListBox() { Size = new Size(400, 200), Location = new Point(150, 630) };
            RandomContentCommand = new GenericCommand((arg) =>
            {
                byte[] tempBuffer = new byte[16];
                DataUnitViewModel._randomSingletone.NextBytes(tempBuffer);
                Units[listBox.SelectedIndex].Content = tempBuffer;
                MessageBox.Show($"Buffer of {Units[listBox.SelectedIndex].Id} filled!");
            });
            ChangeNameCommand = new GenericCommand((arg) =>
            {
                Units[listBox.SelectedIndex].Name += "!";
            });
            this.DataContext = this;
            randomByteGen.DataBindings.Add(new Binding("Command", this.DataContext, "RandomContentCommand", true, DataSourceUpdateMode.OnPropertyChanged));
            changeName.DataBindings.Add(new Binding("Command", this.DataContext, "ChangeNameCommand", true, DataSourceUpdateMode.OnPropertyChanged));
            changeName.DataBindings.Add(new Binding("CommandParameter", forNameChanging, "Text", true, DataSourceUpdateMode.OnPropertyChanged));
            listBox.DataBindings.Add(new Binding("DataSource", this.DataContext, "Units", true, DataSourceUpdateMode.OnPropertyChanged));
            listBox.DisplayMember = "Name";
            listBox.ValueMember = "Content";
            var binding = new Binding("Text", listBox, "SelectedItem", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.FormatString = "dd.mm.YYYY";
            binding.Format += (s, ea) =>
            {
                ea.Value = ea.Value;
            };
            forNameChanging.DataBindings.Add(binding);
            contentBox.DataBindings.Add(new Binding("DataSource", listBox, "SelectedValue", true, DataSourceUpdateMode.OnPropertyChanged));
            Controls.Add(forNameChanging);
            Controls.Add(listBox);
            Controls.Add(randomByteGen);
            Controls.Add(changeName);
            Controls.Add(contentBox);
        }
    }
    public class DataUnitModel
    {
        public Guid? Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Name { get; set; }
        public byte[]? Content { get; set; }
        public override string? ToString()
        {
            return CreatedAt?.ToString();
        }
    }
    public class GenericCommand : ICommand
    {
        private Action<object?> _execute;
        public GenericCommand(Action<object?> execute)
        {
            _execute = execute;
        }
        public void Execute(object? arg) => _execute?.Invoke(arg);
        public bool CanExecute(object? arg) => true;
        public event EventHandler? CanExecuteChanged;
    }
    public class DataUnitViewModel : INotifyPropertyChanged
    {
        internal static Random _randomSingletone = new Random();
        private DataUnitModel? _model;
        public Guid? Id
        {
            get
            {
                return _model?.Id;
            }
            set
            {
                if (_model is not null && value is not null)
                {
                    _model.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime? CreatedAt
        {
            get
            {
                return _model?.CreatedAt;
            }
            set
            {
                if (_model is not null && value is not null)
                {
                    _model.CreatedAt = value;
                    OnPropertyChanged();
                }
            }
        }
        public string? Name
        {
            get
            {
                return _model?.Name;
            }
            set
            {
                if (_model is not null && value is not null)
                {
                    _model.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        public byte[]? Content
        {
            get
            {
                return _model?.Content;
            }
            set
            {
                if (_model is not null && value is not null)
                {
                    _model.Content = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public DataUnitViewModel(DataUnitModel? model)
        {
            if (model is not null)
                _model = model;
            else
                _model = new DataUnitModel();
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
