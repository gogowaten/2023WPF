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
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using static System.Net.WebRequestMethods;
using System.Data;
using System.Collections;
using System.Windows.Markup;

namespace _20230128_Group
{
    public abstract class TThumb : Thumb, INotifyPropertyChanged
    {
        #region プロパティ
        public Data Data { get; set; }
        public DataType DataType { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private TTGroup? _myParent;
        public TTGroup? MyParent { get => _myParent; set => SetProperty(ref _myParent, value); }



        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register(nameof(Left), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }
        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register(nameof(Top), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        #endregion プロパティ
        public TThumb() : this(new Data(DataType.None))
        {

        }
        public TThumb(Data data)
        {
            Data = data;
            DataContext = Data;
            DataType = Data.Type;
            SetBinding(LeftProperty, nameof(Data.X));
            SetBinding(TopProperty, nameof(Data.Y));
            SetBinding(Canvas.LeftProperty, nameof(Data.X));
            SetBinding(Canvas.TopProperty, nameof(Data.Y));

        }
        public override string ToString()
        {
            //return base.ToString();
            return $"{DataType}_{Name}";
        }
    }

    [ContentProperty(nameof(Children))]
    public class TTGroup : TThumb
    {
        public ObservableCollection<TThumb> Children { get; set; } = new();

        private TThumb? _activeThumb;
        public TThumb? ActiveThumb { get => _activeThumb; set => SetProperty(ref _activeThumb, value); }

        private TThumb? _clickedThumb;
        public TThumb? ClickedThumb { get => _clickedThumb; set => SetProperty(ref _clickedThumb, value); }


        public TTGroup() : this(new Data(DataType.Group)) { }
        public TTGroup(Data data) : base(data)
        {
            Data = data;
            Children.CollectionChanged += Children_CollectionChanged;
            SetTemplate();
        }
        private void SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(ItemsControl));
            factory.SetValue(ItemsControl.ItemsPanelProperty,
                new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas))));
            factory.SetValue(ItemsControl.ItemsSourceProperty, new Binding(nameof(Children)) { Source = this });
            this.Template = new() { VisualTree = factory };
        }
        private void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is TThumb tt)
                    {
                        tt.MyParent = this;
                    }
                    break;
                default: break;
            }
        }

        #region サイズと位置の更新

        //TTGroupのRect取得
        public static (double x, double y, double w, double h) GetRect(TTGroup? group)
        {
            if (group == null) { return (0, 0, 0, 0); }
            return GetRect(group.Children);
        }
        public static (double x, double y, double w, double h) GetRect(IEnumerable<TThumb> thumbs)
        {
            double x = double.MaxValue, y = double.MaxValue;
            double w = 0, h = 0;
            if (thumbs != null)
            {
                foreach (var item in thumbs)
                {
                    var left = item.Left; if (x > left) x = left;
                    var top = item.Top; if (y > top) y = top;
                    var width = left + item.ActualWidth;
                    var height = top + item.ActualHeight;
                    if (w < width) w = width;
                    if (h < height) h = height;
                }
            }
            return (x, y, w, h);
        }

        /// <summary>
        /// サイズと位置の更新
        /// </summary>
        public void TTGroupUpdateLayout()
        {
            //Rect取得
            (double x, double y, double w, double h) = GetRect(this);

            //子要素位置修正
            foreach (var item in Children)
            {
                item.Left -= x;
                item.Top -= y;
            }
            //自身がRoot以外なら自身の位置を更新
            if (this.GetType() != typeof(TTRoot))
            {
                Left += x;
                Top += y;
            }

            //自身のサイズ更新
            w -= x; h -= y;
            if (w < 0) w = 0;
            if (h < 0) h = 0;
            if (w >= 0) Width = w;
            if (h >= 0) Height = h;

            //必要、これがないと見た目が変化しない、実行直後にSizeChangedが発生
            UpdateLayout();

            //親要素Groupがあれば遡って更新
            if (MyParent is TTGroup parent)
            {
                parent.TTGroupUpdateLayout();
            }
        }
        #endregion サイズと位置の更新

    }

    public class TTRoot : TTGroup
    {
        //クリック前の選択状態、クリックUp時の削除に使う
        private bool IsSelectedPreviewMouseDown { get; set; }
        public ObservableCollection<TThumb> SelectedThumbs { get; set; } = new();

        private TTGroup _activeGroup;
        public TTGroup ActiveGroup { get => _activeGroup; set => SetProperty(ref _activeGroup, value); }

        public TTRoot() : this(new Data(DataType.Root))
        {
        }
        public TTRoot(Data data) : base(data)
        {
            _activeGroup = this;
            Loaded += TTRoot_Loaded;

        }

        private void TTRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (Children.Count > 0)
            {
                foreach (var item in Children)
                {
                    Data.ChildrenData.Add(item.Data);
                    item.DragDelta += Thumb_DragDelta;
                    item.DragStarted += Thumb_DragStarted;
                    item.DragCompleted += Thumb_DragCompleted;
                }
            }
            TTGroupUpdateLayout();
        }

        #region ドラッグ移動


        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {

        }

        private void Thumb_DragDelta(object seneer, DragDeltaEventArgs e)
        {
            //複数選択時は全てを移動
            foreach (TThumb item in SelectedThumbs)
            {
                item.Data.X += e.HorizontalChange;
                item.Data.Y += e.VerticalChange;
            }
        }
        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (sender is TThumb thumb) { thumb.MyParent?.TTGroupUpdateLayout(); }
        }
        #endregion ドラッグ移動

        #region オーバーライド関連

        //起動直後、自身がActiveGroupならChildrenにドラッグ移動登録
        //protected override void OnInitialized(EventArgs e)
        //{
        //    base.OnInitialized(e);
        //    if (ActiveGroup == this)
        //    {
        //        foreach (var item in Children)
        //        {
        //            item.DragDelta += Thumb_DragDelta;
        //            item.DragCompleted += Thumb_DragCompleted;
        //            item.DragStarted += Thumb_DragStarted;
        //        }
        //    }
        //    //TTGroupUpdateLayout();
        //}

        //クリックしたとき、ClickedThumbの更新とActiveThumbの更新、SelectedThumbsの更新
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);//要る？

            //OriginalSourceにテンプレートに使っている要素が入っているので、
            //そのTemplateParentプロパティから目的のThumbが取得できる
            if (e.OriginalSource is FrameworkElement el && el.TemplatedParent is TThumb clicked)
            {
                ClickedThumb = clicked;
                TThumb? active = GetActiveThumb(clicked);
                if (active != ActiveThumb)
                {
                    ActiveThumb = active;
                }
                //SelectedThumbsの更新
                if (active != null)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        if (SelectedThumbs.Contains(active) == false)
                        {
                            SelectedThumbs.Add(active);
                            IsSelectedPreviewMouseDown = false;
                        }
                        else
                        {
                            //フラグ
                            //ctrl+クリックされたものがもともと選択状態だったら
                            //マウスアップ時に削除するためのフラグ
                            IsSelectedPreviewMouseDown = true;
                        }
                    }
                    else
                    {
                        if (SelectedThumbs.Contains(active) == false)
                        {
                            SelectedThumbs.Clear();
                            SelectedThumbs.Add(active);
                            IsSelectedPreviewMouseDown = false;
                        }
                    }
                }
                else { IsSelectedPreviewMouseDown = false; }
            }
        }

        //マウスレフトアップ、フラグがあればSelectedThumbsから削除する
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //
            if (SelectedThumbs.Count > 1 && IsSelectedPreviewMouseDown && ActiveThumb != null)
            {
                SelectedThumbs.Remove(ActiveThumb);//削除
                IsSelectedPreviewMouseDown = false;//フラグ切り替え
                ActiveThumb = SelectedThumbs[^1];
                ClickedThumb = SelectedThumbs[^1];
                //ActiveThumb = null;
                //ClickedThumb = null;
            }

        }

        #endregion オーバーライド関連

        #region その他関数


        /// <summary>
        /// ActiveThumb変更時に実行、FrontActiveThumbとBackActiveThumbを更新する
        /// </summary>
        /// <param name="value"></param>
        //private void ChangedActiveThumb(TThumb? value)
        //{
        //    if (value == null)
        //    {
        //        FrontActiveThumb = null;
        //        BackActiveThumb = null;
        //    }
        //    else
        //    {
        //        int ii = ActiveGroup.Thumbs.IndexOf(value);
        //        int tc = ActiveGroup.Thumbs.Count;

        //        if (tc <= 1)
        //        {
        //            FrontActiveThumb = null;
        //            BackActiveThumb = null;
        //        }
        //        else
        //        {
        //            if (ii - 1 >= 0)
        //            {
        //                BackActiveThumb = ActiveGroup.Thumbs[ii - 1];
        //            }
        //            else
        //            {
        //                BackActiveThumb = null;
        //            }
        //            if (ii + 1 >= tc)
        //            {
        //                FrontActiveThumb = null;
        //            }
        //            else
        //            {
        //                FrontActiveThumb = ActiveGroup.Thumbs[ii + 1];
        //            }
        //        }
        //    }
        //}

        private bool CheckIsActive(TThumb thumb)
        {
            if (thumb.MyParent is TTGroup ttg && ttg == ActiveGroup)
            {
                return true;
            }
            return false;

        }
        //起点からActiveThumbをサーチ
        //ActiveはActiveThumbのChildrenの中で起点に連なるもの
        private TThumb? GetActiveThumb(TThumb? start)
        {
            if (start == null) { return null; }
            if (CheckIsActive(start))
            {
                return start;
            }
            else if (start.MyParent is TTGroup ttg)
            {
                return GetActiveThumb(ttg);
            }
            return null;
        }
        /// <summary>
        /// SelectedThumbsを並べ替えたList作成、基準はActiveGroupのChildren
        /// </summary>
        /// <param name="selected">SelectedThumbs</param>
        /// <param name="group">並べ替えの基準にするGroup</param>
        /// <returns></returns>
        private List<TThumb> MakeSortedList(IEnumerable<TThumb> selected, TTGroup group)
        {
            List<TThumb> tempList = new();
            foreach (var item in group.Children)
            {
                if (selected.Contains(item)) { tempList.Add(item); }
            }
            return tempList;
        }
        private List<TThumb> MakeSortedList()
        {
            List<TThumb> tempList = new();
            foreach (var item in ActiveGroup.Children)
            {
                if (SelectedThumbs.Contains(item)) { tempList.Add(item); }
            }
            return tempList;
        }

        /// <summary>
        /// 要素すべてがGroupのChildrenに存在するか判定
        /// </summary>
        /// <param name="thums">要素群</param>
        /// <param name="group">ParentGroup</param>
        /// <returns></returns>
        private bool IsAllContains(IEnumerable<TThumb> thums, TTGroup group)
        {
            if (!thums.Any()) { return false; }//要素が一つもなければfalse
            foreach (var item in thums)
            {
                if (group.Children.Contains(item) == false)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// グループ化の条件が揃っているのかの判定
        /// </summary>
        /// <param name="thumbs">グループ化要素群</param>
        /// <param name="destGroup">グループ追加先グループ</param>
        /// <returns></returns>
        private static bool CheckAddGroup(IEnumerable<TThumb> thumbs, TTGroup destGroup)
        {
            //要素群数が2以上
            //要素群数が追加先グループの子要素数より少ない、ただし
            //追加先グループがTTRootなら子要素数と同じでもいい
            //要素群すべてが追加先グループの子要素
            //これらすべてを満たした場合はtrue
            if (thumbs.Count() < 2) { return false; }
            if (thumbs.Count() == destGroup.Children.Count)
            {
                if (destGroup.DataType == DataType.Root) { return true; }
                else { return false; }
            }
            foreach (TThumb thumb in thumbs)
            {
                if (destGroup.Children.Contains(thumb) == false) { return false; }
            }
            return true;
        }
        private bool CheckAddGroup()
        {
            if (SelectedThumbs.Count < 2) { return false; }
            if (SelectedThumbs.Count == ActiveGroup.Children.Count)
            {
                if (ActiveGroup.DataType == DataType.Root) { return true; }
                else { return false; }
            }
            foreach (TThumb thumb in SelectedThumbs)
            {
                if (ActiveGroup.Children.Contains(thumb) == false) { return false; }
            }
            return true;
        }

        #endregion その他関数

        /// <summary>
        /// SelectedThumbからGroup作成してActiveGroupに追加する
        /// </summary>
        public void AddGroup()
        {
            if (CheckAddGroup() == false) return;
            //新グループの挿入Index、[^1]は末尾から数えて1番目の要素って意味
            var sortedList = MakeSortedList();
            int insertIndex = ActiveGroup.Children.IndexOf(sortedList[^1]) - (sortedList.Count - 1);
            var (x, y, w, h) = GetRect(sortedList);
            TTGroup newGroup = new()
            {
                Left = x,
                Top = y,
                //TTGrid = destGroup.TTGrid,
                //TTXShift = destGroup.TTXShift,
                //TTYShift = destGroup.TTYShift,
            };
            ActiveGroup.Children.Insert(insertIndex, newGroup);
            ActiveGroup.Data.ChildrenData.Insert(insertIndex, newGroup.Data);
            newGroup.DragDelta += Thumb_DragDelta;
            newGroup.DragStarted += Thumb_DragStarted;
            newGroup.DragCompleted += Thumb_DragCompleted;

            foreach (var item in sortedList)
            {
                ActiveGroup.Children.Remove(item);
                ActiveGroup.Data.ChildrenData.Remove(item.Data);
                SelectedThumbs.Remove(item);
                item.DragDelta -= Thumb_DragDelta;
                item.DragStarted -= Thumb_DragStarted;
                item.DragCompleted -= Thumb_DragCompleted;
                item.Left -= x; item.Top -= y;
            }

            foreach (var item in sortedList)
            {
                newGroup.Children.Add(item);
                newGroup.Data.ChildrenData.Add(item.Data);

            }
        }
    }

    public class TTTextBlock : TThumb
    {

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TTTextBlock),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public Brush Background
        //{
        //    get { return (Brush)GetValue(BackgroundProperty); }
        //    set { SetValue(BackgroundProperty, value); }
        //}
        //public static readonly DependencyProperty BackgroundProperty =
        //    DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(TTTextBlock),
        //        new FrameworkPropertyMetadata(Brushes.Transparent,
        //            FrameworkPropertyMetadataOptions.AffectsRender |
        //            FrameworkPropertyMetadataOptions.AffectsMeasure));

        public TTTextBlock() : this(new Data(DataType.TextBlock))
        {

        }
        public TTTextBlock(Data data) : base(data)
        {
            SetBinding(TextProperty, nameof(Data.Text));
            //SetBinding(BackgroundProperty, new Binding(nameof(Data.Backgound)) { Source=this, Mode = BindingMode.TwoWay});
            SetTemplate();
        }
        private void SetTemplate()
        {
            FrameworkElementFactory factory = new(typeof(TextBlock));
            factory.SetValue(TextBlock.TextProperty, new Binding(nameof(Data.Text)));
            factory.SetValue(TextBlock.BackgroundProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(Thumb.BackgroundProperty),
                Mode = BindingMode.TwoWay
            });
            //SetBinding(BackgroundProperty, new Binding(nameof(Data.Backgound))
            //{
            //    Source = this,
            //    Mode =BindingMode.TwoWay
            //});
            //factory.SetValue(TextBlock.BackgroundProperty, new Binding()
            //{
            //    Source = this,
            //    Path = new PropertyPath(Thumb.BackgroundProperty),
            //    Mode = BindingMode.TwoWay
            //});

            this.Template = new() { VisualTree = factory };
        }
    }
}
