using Autofac;
using AutoMasshTik.Engine;
using AutoMasshTik.Engine.Actions;
using AutoMasshTik.UI.Converters;
using AutoMasshTik.UI.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Data;
using Avalonia.Media;
using Avalonia.Styling;
using PropertyChanged;

namespace AutoMasshTik.UI
{
    [DoNotNotify]
    public class MainWindow : Window
    {
        readonly ILifetimeScope scope;
        public MainViewModel ViewModel { get; }
        public MainWindow()
        {
            scope = IoCRegistrar.Container.BeginLifetimeScope();
            ViewModel = scope.Resolve<MainViewModel>();
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = ViewModel;
            ViewModel.Start();
        }

        FuncDataTemplate<ServerViewModel> CreateServerTemplate()
        {
            return new FuncDataTemplate<ServerViewModel>(x =>

                new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions("100,80,*")
                }.AddChildren(
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding(nameof(ServerViewModel.Url)),
                    },
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding(nameof(ServerViewModel.ServerState)) { Converter = ServerUpdateStateToStringConverter.Default },
                        [!TextBlock.ForegroundProperty] = new Binding(nameof(ServerViewModel.ServerState)) {  Converter = ServerUpdateStateToBrushConverter.Default },
                    }.SetGridColumn(1),
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding(nameof(ServerViewModel.Error)),
                    }.SetGridColumn(2)
                ));
        }
        void InitializeComponent()
        {
            Title = "AutoMasshTik";
            Height = MinHeight = 300;
            Width = MinWidth = 830;
            SolidColorBrush defaultBorderBrush = new SolidColorBrush(Colors.Black, opacity: .4);
            double defaultBorderThickness = .5;
            Styles.Add(
                new Style(s => s.OfType<TextBox>().Class("input"))
                    .AddSetters(
                        new Setter(TextBox.BorderThicknessProperty, defaultBorderThickness),
                        new Setter(TextBox.BorderBrushProperty, defaultBorderBrush),
                        new Setter(TextBox.UseFloatingWatermarkProperty, true)
                    ));
            //AvaloniaXamlLoader.Load(this);
            Content = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto, *, Auto"),
                ColumnDefinitions = new ColumnDefinitions("200,5,300,5,*"),
                Margin = new Thickness(10)
            }
            .AddChildren(
                new TextBox
                {
                    [!TextBox.TextProperty] = new Binding(nameof(ViewModel.ServersText), BindingMode.TwoWay),
                    AcceptsReturn = true,
                    Watermark = "Addresses",
                    [!Button.IsEnabledProperty] = new Binding(nameof(ViewModel.IsUpdating)) { Converter = NegateConverter.Default }
                }.AddClass("input").SetGridRowSpan(3),
                new StackPanel
                {
                }.SetGridColumn(2)
                .AddChildren(
                    new TextBox
                    {
                        [!TextBox.TextProperty] = new Binding(nameof(ViewModel.Username), BindingMode.TwoWay),
                        [!TextBox.BorderBrushProperty] = new Binding(nameof(ViewModel.Username)) { Converter = NotEmptyRequiredToBrushConverter.Default },
                        Watermark = "Username",
                        [!TextBlock.IsEnabledProperty] = new Binding(nameof(ViewModel.IsUpdating)) { Converter = NegateConverter.Default }

                    }.AddClass("input"),
                     new TextBox
                     {
                         [!TextBox.TextProperty] = new Binding(nameof(ViewModel.Password), BindingMode.TwoWay),
                         [!TextBox.BorderBrushProperty] = new Binding(nameof(ViewModel.Password)) { Converter = NotEmptyRequiredToBrushConverter.Default },
                         Watermark = "Password",
                         Margin = new Thickness(0, 5, 0, 0),
                         [!TextBlock.IsEnabledProperty] = new Binding(nameof(ViewModel.IsUpdating)) { Converter = NegateConverter.Default }
                     }.AddClass("input"),
                     new TextBox
                     {
                         [!TextBox.TextProperty] = new Binding(nameof(ViewModel.Port), BindingMode.TwoWay) { Converter = IntToStringConverter.Default },
                         Watermark = "Port",
                         Margin = new Thickness(0, 5, 0, 0),
                         Width = 40,
                         HorizontalAlignment = HorizontalAlignment.Left,
                         [!TextBlock.IsEnabledProperty] = new Binding(nameof(ViewModel.IsUpdating)) { Converter = NegateConverter.Default }
                     }.AddClass("input")
                    ),
                new StackPanel
                {
                    VerticalAlignment  = VerticalAlignment.Center,
                    Orientation = Orientation.Vertical,
                    [!StackPanel.IsVisibleProperty] = new Binding(nameof(ViewModel.IsUpdating), BindingMode.OneWay)
                }.SetGridColumn(2).SetGridRow(1)
                .AddChildren(
                    new TextBlock
                    {
                        [!TextBlock.TextProperty] = new Binding(nameof(ViewModel.OperationInProgress)),
                        HorizontalAlignment = HorizontalAlignment.Center
                    },
                    new ProgressBar
                    {
                        [!ProgressBar.ValueProperty] = new Binding(nameof(ViewModel.ServerModels), BindingMode.OneWay) {  Converter = ServersToProgressConverter.Default },
                        [!ProgressBar.MaximumProperty] = new Binding(nameof(ViewModel.ServerModels), BindingMode.OneWay) { Converter = ServersToProgressMaxConverter.Default }
                    }),
                new StackPanel
                {
                    Orientation = Orientation.Vertical,
                }.SetGridColumn(2).SetGridRow(2)
                .AddChildren(
                    new Button
                    {
                        Content = "Test connection",
                        [!Button.IsVisibleProperty] = new Binding(nameof(ViewModel.IsUpdating)) { Converter = NegateConverter.Default },
                        [!Button.CommandProperty] = new Binding(nameof(ViewModel.StartUpdateCommand)),
                        CommandParameter = UpdateMode.Connection
                    },
                    new Button
                    {
                        Content = "Update Packages",
                        Margin = new Thickness(0, 5, 0, 0),
                        [!Button.IsVisibleProperty] = new Binding(nameof(ViewModel.IsUpdating)) { Converter = NegateConverter.Default },
                        [!Button.CommandProperty] = new Binding(nameof(ViewModel.StartUpdateCommand)),
                        CommandParameter = UpdateMode.Packages
                    },
                    new Button
                    {
                        Content = "Update Firmware And Reboot",
                        Margin = new Thickness(0, 5, 0, 0),
                        [!Button.IsVisibleProperty] = new Binding(nameof(ViewModel.IsUpdating)) { Converter = NegateConverter.Default },
                        [!Button.CommandProperty] = new Binding(nameof(ViewModel.StartUpdateCommand)),
                        CommandParameter = UpdateMode.Firmware
                    },
                    new Button
                    {
                        Content = "Cancel",
                        [!Button.IsVisibleProperty] = new Binding(nameof(ViewModel.IsUpdating)),
                        [!Button.CommandProperty] = new Binding(nameof(ViewModel.StopUpdateCommand))
                    }),
                new ListBox
                {
                    BorderThickness = .8,
                    BorderBrush = defaultBorderBrush,
                    [!ListBox.ItemsProperty] = new Binding(nameof(ViewModel.Servers)),
                    ItemTemplate = CreateServerTemplate(),
                }.SetGridRowSpan(3).SetGridColumn(4)
                );
        }
        protected override void HandleClosed()
        {
            scope.Dispose();
            base.HandleClosed();
        }
    }
}
