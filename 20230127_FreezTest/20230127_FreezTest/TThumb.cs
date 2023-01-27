using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20230127_FreezTest
{
    [DebuggerDisplay("Type = {" + nameof(Type) + "}")]
    public abstract class TThumb : Thumb
    {
        //依存プロパティは主にデザイナー画面で、要素を追加して表示の確認する用
        #region 依存プロパティ

        public double TTLeft
        {
            get { return (double)GetValue(TTLeftProperty); }
            set { SetValue(TTLeftProperty, value); }
        }
        public static readonly DependencyProperty TTLeftProperty =
            DependencyProperty.Register(nameof(TTLeft), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double TTTop
        {
            get { return (double)GetValue(TTTopProperty); }
            set { SetValue(TTTopProperty, value); }
        }
        public static readonly DependencyProperty TTTopProperty =
            DependencyProperty.Register(nameof(TTTop), typeof(double), typeof(TThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存プロパティ
        protected readonly string TEMPLATE_NAME = "NEMO";
        public TTGroup? TTParent { get; set; } = null;//親Group
        public TType Type { get; private set; }
        public Data Data { get; set; }// = new(TType.None);

        public TThumb() : this(new Data(TType.None)) { }
        public TThumb(Data data)
        {

            SetBinding(Canvas.LeftProperty, new Binding()
            {
                Path = new PropertyPath(TTLeftProperty),
                Source = this
            });
            SetBinding(Canvas.TopProperty, new Binding()
            {
                Path = new PropertyPath(TTTopProperty),
                Source = this
            });
            Type = data.Type;
            this.Data = data;
            DataContext = data;
            SetBinding(TTLeftProperty, nameof(data.X));
            SetBinding(TTTopProperty, nameof(data.Y));

        }

        //サイズ変更時には親要素の位置とサイズ更新
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            TTParent?.TTGroupUpdateLayout();
        }

        protected T? MakeTemplate<T>()
        {
            FrameworkElementFactory factory = new(typeof(T), TEMPLATE_NAME);
            this.Template = new() { VisualTree = factory };
            this.ApplyTemplate();
            if (this.Template.FindName(TEMPLATE_NAME, this) is T element)
            {
                return element;
            }
            else return default;
        }

    }




    //[DebuggerDisplay("Name = {" + nameof(Name) + "}")]
    [ContentProperty(nameof(Thumbs))]
    public class TTGroup : TThumb
    {
        #region 依存プロパティ

        public int TTXShift
        {
            get { return (int)GetValue(TTXShiftProperty); }
            set { SetValue(TTXShiftProperty, value); }
        }
        public static readonly DependencyProperty TTXShiftProperty =
            DependencyProperty.Register(nameof(TTXShift), typeof(int), typeof(TTGroup),
                new FrameworkPropertyMetadata(32,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int TTYShift
        {
            get { return (int)GetValue(TTYShiftProperty); }
            set { SetValue(TTYShiftProperty, value); }
        }
        public static readonly DependencyProperty TTYShiftProperty =
            DependencyProperty.Register(nameof(TTYShift), typeof(int), typeof(TTGroup),
                new FrameworkPropertyMetadata(32,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int TTGrid
        {
            get { return (int)GetValue(TTGridProperty); }
            set { SetValue(TTGridProperty, value); }
        }
        public static readonly DependencyProperty TTGridProperty =
            DependencyProperty.Register(nameof(TTGrid), typeof(int), typeof(TTGroup),
                new FrameworkPropertyMetadata(8,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Visibility TTVisibleBorder
        {
            get { return (Visibility)GetValue(VisibleBorderProperty); }
            set { SetValue(VisibleBorderProperty, value); }
        }
        public static readonly DependencyProperty VisibleBorderProperty =
            DependencyProperty.Register(nameof(TTVisibleBorder), typeof(Visibility), typeof(TTGroup),
                new FrameworkPropertyMetadata(Visibility.Hidden,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存プロパティ
        private ItemsControl MyTemplateElement;
        public ObservableCollection<TThumb> Thumbs { get; private set; } = new();



        public TTGroup() : this(new Data(TType.Group))
        {

        }
        public TTGroup(Data data) : base(data)
        {
            //初期設定
            Thumbs.CollectionChanged += Thumbs_CollectionChanged;
            Data = data;
            MyTemplateElement = MyInitializeBinding();
            MyTemplateElement.SetBinding(ItemsControl.ItemsSourceProperty,
                new Binding(nameof(Thumbs)) { Source = this });

            SetBinding(TTXShiftProperty, nameof(Data.XShift));
            SetBinding(TTYShiftProperty, nameof(Data.YShift));
            SetBinding(TTGridProperty, nameof(Data.Grid));

        }

        private void Thumbs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is TThumb thumb)
                    {
                        thumb.TTParent = this;
                    }
                    break;
                default: break;
            }
        }

        private ItemsControl MyInitializeBinding()
        {
            this.DataContext = Data;
            return MakeTemplate();

        }
        private ItemsControl MakeTemplate()
        {
            FrameworkElementFactory fGrid = new(typeof(Grid));
            FrameworkElementFactory fWaku = new(typeof(Rectangle));

            fWaku.SetValue(VisibilityProperty, new Binding(nameof(TTVisibleBorder)) { Source = this });
            fWaku.SetValue(Shape.StrokeProperty, Brushes.Red);
            fWaku.SetValue(Shape.StrokeThicknessProperty, 10.0);
            FrameworkElementFactory factory = new(typeof(ItemsControl), TEMPLATE_NAME);
            factory.SetValue(ItemsControl.ItemsPanelProperty,
                new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas))));
            fGrid.AppendChild(fWaku);
            fGrid.AppendChild(factory);
            this.Template = new() { VisualTree = fGrid };
            this.ApplyTemplate();
            if (this.Template.FindName(TEMPLATE_NAME, this) is ItemsControl element)
            {
                return element;
            }
            else { throw new ArgumentException("テンプレート作成できんかった"); }
        }
        //private ItemsControl MakeTemplate()
        //{
        //    FrameworkElementFactory factory = new(typeof(ItemsControl), TEMPLATE_NAME);
        //    factory.SetValue(ItemsControl.ItemsPanelProperty,
        //        new ItemsPanelTemplate(new FrameworkElementFactory(typeof(Canvas))));
        //    this.Template = new() { VisualTree = factory };
        //    this.ApplyTemplate();
        //    if (this.Template.FindName(TEMPLATE_NAME, this) is ItemsControl element)
        //    {
        //        return element;
        //    }
        //    else { throw new ArgumentException("テンプレート作成できんかった"); }
        //}

        #region サイズと位置の更新

        //TTGroupのRect取得
        public static (double x, double y, double w, double h) GetRect(TTGroup? group)
        {
            if (group == null) { return (0, 0, 0, 0); }
            return GetRect(group.Thumbs);
        }
        public static (double x, double y, double w, double h) GetRect(IEnumerable<TThumb> thumbs)
        {
            double x = double.MaxValue, y = double.MaxValue;
            double w = 0, h = 0;
            if (thumbs != null)
            {
                foreach (var item in thumbs)
                {
                    var left = item.TTLeft; if (x > left) x = left;
                    var top = item.TTTop; if (y > top) y = top;
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
            foreach (var item in Thumbs)
            {
                item.TTLeft -= x;
                item.TTTop -= y;
            }
            //自身がRoot以外なら自身の位置を更新
            if (this.GetType() != typeof(TTRoot))
            {
                TTLeft += x;
                TTTop += y;
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
            if (TTParent is TTGroup parent)
            {
                parent.TTGroupUpdateLayout();
            }
        }
        #endregion サイズと位置の更新

    }


    public class TTRoot : TTGroup, INotifyPropertyChanged
    {

        #region 通知プロパティ

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        //最後にクリックしたThumb
        private TThumb? _clickedThumb;
        public TThumb? ClickedThumb { get => _clickedThumb; set => SetProperty(ref _clickedThumb, value); }

        //注目しているThumb、選択Thumb群の筆頭
        private TThumb? _activeThumb;
        public TThumb? ActiveThumb
        {
            get => _activeThumb;
            set
            {
                SetProperty(ref _activeThumb, value);
                //FrontActiveThumbとBackActiveThumbを更新する
                ChangedActiveThumb(value);
            }
        }

        private TThumb? _frontActiveThumb;
        public TThumb? FrontActiveThumb { get => _frontActiveThumb; set => SetProperty(ref _frontActiveThumb, value); }

        private TThumb? _backActiveThumb;
        public TThumb? BackActiveThumb { get => _backActiveThumb; set => SetProperty(ref _backActiveThumb, value); }

        //ActiveThumbの親要素、移動可能なThumbはこの要素の中のThumbだけ。起動直後はRootThumbがこれになる
        private TTGroup _activeGroup;
        public TTGroup ActiveGroup
        {
            get => _activeGroup;
            set
            {
                ChildrenDragEventDesoption(_activeGroup, value);
                SetProperty(ref _activeGroup, value);
            }
        }

        #endregion 通知プロパティ

        //選択状態の要素を保持
        public ObservableCollection<TThumb> SelectedThumbs { get; private set; } = new();

        //クリック前の選択状態、クリックUp時の削除に使う
        private bool IsSelectedPreviewMouseDown { get; set; }

        #region コンストラクタ

        public TTRoot() : base(new Data(TType.Root))
        {
            _activeGroup ??= this;

            //起動直後に位置とサイズ更新
            //TTGroupUpdateLayout();//XAML上でThumb設置しても、この時点ではThumbsが0個
            Loaded += (a, b) =>
            {
                TTGroupUpdateLayout();
                FixDataDatas();
            };

            //SetBinding(TTXShiftProperty, nameof(Data.XShift));
            //SetBinding(TTYShiftProperty, nameof(Data.YShift));
            //SetBinding(TTGridProperty, nameof(Data.Grid));

        }

        private void FixDataDatas()
        {
            if (Data.Datas == null) return;
            foreach (var item in Thumbs)
            {
                if (Data.Datas.Contains(item.Data) == false)
                {
                    Data.Datas.Add(item.Data);
                }
            }
        }

        #endregion コンストラクタ

        /// <summary>
        /// RootDataをセット、初期化
        /// </summary>
        /// <param name="data"></param>
        public void SetRootData(Data data)
        {
            //初期化
            if (data.Type != TType.Root) return;
            Thumbs.Clear();
            SelectedThumbs.Clear();
            this.Data = data;
            DataContext = this.Data;
            ActiveGroup = this;
            ClickedThumb = null;
            ActiveThumb = null;

            //子要素追加
            foreach (var item in data.Datas)
            {
                TThumb thumb = BuildThumb(item);
                Thumbs.Add(thumb);
                //直下のThumbにはドラッグ移動イベント付加
                thumb.DragDelta += Thumb_DragDelta;
                thumb.DragCompleted += Thumb_DragCompleted;
                thumb.DragStarted += Thumb_DragStarted;
                //追加子要素がGroupだった場合
                if (thumb is TTGroup group)
                {
                    SetData(group);
                }
            }
        }


        //GroupのThumbsに子要素追加
        private void SetData(TTGroup group)
        {
            foreach (Data data in group.Data.Datas)
            {
                TThumb thumb = BuildThumb(data);
                group.Thumbs.Add(thumb);
                //newGroup.Data.Datas.Add(thumb.Data);
                if (thumb is TTGroup gThumb)
                {
                    SetData(gThumb);
                }
            }
        }


        #region ドラッグ移動

        //ActiveGroup用、ドラッグ移動イベント脱着
        private void ChildrenDragEventDesoption(TTGroup removeTarget, TTGroup addTarget)
        {
            foreach (var item in removeTarget.Thumbs)
            {
                item.DragDelta -= Thumb_DragDelta;
                item.DragCompleted -= Thumb_DragCompleted;
                item.DragStarted -= Thumb_DragStarted;
            }
            foreach (var item in addTarget.Thumbs)
            {
                item.DragDelta += Thumb_DragDelta;
                item.DragCompleted += Thumb_DragCompleted;
                item.DragStarted += Thumb_DragStarted;
            }
        }
        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            //グリッドスナップ
            if (sender is TThumb thumb)
            {
                int gridSize = ActiveGroup.Data.Grid;
                thumb.TTLeft = (int)(thumb.TTLeft / gridSize + 0.5) * gridSize;
                thumb.TTTop = (int)(thumb.TTTop / gridSize + 0.5) * gridSize;
            }
        }

        private void Thumb_DragDelta(object seneer, DragDeltaEventArgs e)
        {
            //複数選択時は全てを移動
            foreach (TThumb item in SelectedThumbs)
            {
                double hc = e.HorizontalChange;
                int gridSize = ActiveGroup.Data.Grid;
                if (hc > 0)
                {
                    item.TTLeft += (int)(hc / gridSize + 0.5) * gridSize;
                }
                else if (hc < 0)
                {
                    item.TTLeft += (int)(hc / gridSize - 0.5) * gridSize;
                }

                double vc = e.VerticalChange;
                if (vc > 0)
                {
                    item.TTTop += (int)(vc / gridSize + 0.5) * gridSize;
                }
                else if (vc < 0)
                {
                    item.TTTop += (int)(vc / gridSize - 0.5) * gridSize;
                }
            }
        }
        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (sender is TThumb thumb) { thumb.TTParent?.TTGroupUpdateLayout(); }
        }
        #endregion ドラッグ移動

        #region オーバーライド関連

        //起動直後、自身がActiveGroupならChildrenにドラッグ移動登録
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (ActiveGroup == this)
            {
                foreach (var item in Thumbs)
                {
                    item.DragDelta += Thumb_DragDelta;
                    item.DragCompleted += Thumb_DragCompleted;
                    item.DragStarted += Thumb_DragStarted;
                }
            }
            //TTGroupUpdateLayout();
        }

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
        private void ChangedActiveThumb(TThumb? value)
        {
            if (value == null)
            {
                FrontActiveThumb = null;
                BackActiveThumb = null;
            }
            else
            {
                int ii = ActiveGroup.Thumbs.IndexOf(value);
                int tc = ActiveGroup.Thumbs.Count;

                if (tc <= 1)
                {
                    FrontActiveThumb = null;
                    BackActiveThumb = null;
                }
                else
                {
                    if (ii - 1 >= 0)
                    {
                        BackActiveThumb = ActiveGroup.Thumbs[ii - 1];
                    }
                    else
                    {
                        BackActiveThumb = null;
                    }
                    if (ii + 1 >= tc)
                    {
                        FrontActiveThumb = null;
                    }
                    else
                    {
                        FrontActiveThumb = ActiveGroup.Thumbs[ii + 1];
                    }
                }
            }
        }

        private bool CheckIsActive(TThumb thumb)
        {
            if (thumb.TTParent is TTGroup ttg && ttg == ActiveGroup)
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
            else if (start.TTParent is TTGroup ttg)
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
            foreach (var item in group.Thumbs)
            {
                if (selected.Contains(item)) { tempList.Add(item); }
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
                if (group.Thumbs.Contains(item) == false)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion その他関数

        #region 追加と削除

        /// <summary>
        /// 追加先Groupを指定して追加、挿入Indexは最後尾(最前面)
        /// </summary>
        /// <param name="thumb">追加する子要素</param>
        /// <param name="destGroup">追加先Group</param>
        protected void AddThumb(TThumb thumb, TTGroup destGroup)
        {
            AddThumb(thumb, destGroup, destGroup.Thumbs.Count);
            //AddThumbToActiveGroup(thumb, destGroup, destGroup.Thumbs.Count - 1);
        }
        /// <summary>
        /// グループ化で使用、追加先Groupと挿入Indexを指定して追加
        /// </summary>
        /// <param name="thumb">追加する子要素</param>
        /// <param name="destGroup">追加先Group</param>
        /// <param name="insert">挿入Index</param>
        protected void AddThumb(TThumb thumb, TTGroup destGroup, int insert)
        {
            destGroup.Thumbs.Insert(insert, thumb);
            destGroup.Data.Datas.Insert(insert, thumb.Data);
            //ドラッグ移動イベント付加
            thumb.DragDelta += Thumb_DragDelta;
            thumb.DragCompleted += Thumb_DragCompleted;
            thumb.DragStarted += Thumb_DragStarted;
        }
        protected void AddThumbWithoutDragEvent(TThumb thumb, TTGroup destGroup)
        {
            destGroup.Thumbs.Add(thumb);
            destGroup.Data.Datas.Add(thumb.Data);
        }

        //基本的にActiveThumbのChildrenに対して行う
        //削除対象はActiveThumbになる
        //ドラッグ移動イベントの着脱も行う
        public void AddThumbToActiveGroup(TThumb thumb)
        {
            AddThumb(thumb, ActiveGroup);
        }
        /// <summary>
        /// ActiveThumbに要素をDataで追加
        /// </summary>
        /// <param name="data"></param>
        public void AddThumbDataToActiveGroup(Data data)
        {
            //data.Guid = Guid.NewGuid().ToString();
            if (BuildThumb(data) is TThumb thumb)
            {
                AddThumb(thumb, ActiveGroup);//直下にはドラッグ移動イベント付加
                //位置修正、追加先のActiveThumbに合わせる
                if (ActiveThumb != null)
                {
                    data.X = ActiveThumb.Data.X + ActiveGroup.TTXShift;
                    data.Y = ActiveThumb.Data.Y + ActiveGroup.TTYShift;
                }
                else
                {
                    data.X = 0;
                    data.Y = 0;
                }
                //Groupだった場合は子要素も追加、子要素にドラッグ移動イベント追加しない
                if (thumb is TTGroup group)
                {
                    SetData(group);
                }
                ActiveThumb = thumb;
                SelectedThumbs.Clear();
                SelectedThumbs.Add(thumb);
                ClickedThumb = thumb;
            }
        }

        /// <summary>
        /// Dataから各種Thumbを構築
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TThumb BuildThumb(Data data)
        {
            switch (data.Type)
            {
                case TType.None:
                    throw new NotImplementedException();
                case TType.Root:
                    throw new NotImplementedException();
                case TType.Group:
                    return new TTGroup(data);
                case TType.TextBlock:
                    return new TTTextBlock(data);
                case TType.Image:
                    return new TTImage(data);
                case TType.Rectangle:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 選択Thumbすべてを削除
        /// </summary>
        /// <returns></returns>
        public bool RemoveThumb()
        {
            if (SelectedThumbs == null) return false;
            bool flag = true;
            int indexNextActThumb = 0;
            foreach (var item in SelectedThumbs.ToArray())
            {
                indexNextActThumb = ActiveGroup.Thumbs.IndexOf(item);
                if (RemoveThumb(item, ActiveGroup) == false)
                {
                    flag = false;
                    break;
                }
                else
                {
                    SelectedThumbs.Remove(item);
                }
            }

            //削除後の処理、ActiveThumbの指定とSelectedThumbsの指定
            //削除失敗時
            if (flag == false)
            {
                ActiveThumb = null;
            }
            //削除成功時
            else
            {
                //次のActiveThumbは削除されたThumbの下にあるThumb、ただし
                //それがない場合は上のThumb、これもない場合はnull
                if (indexNextActThumb > 0)
                {
                    indexNextActThumb--;
                }

                if (ActiveGroup.Thumbs.Count == 0)
                {
                    ActiveThumb = null;
                }
                else
                {
                    ActiveThumb = ActiveGroup.Thumbs[indexNextActThumb];
                    SelectedThumbs.Add(ActiveThumb);
                }
            }

            ClickedThumb = null;
            return flag;
        }
        /// <summary>
        /// 指定Thumbだけを指定Groupから削除
        /// </summary>
        /// <param name="thumb"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool RemoveThumb(TThumb thumb, TTGroup group)
        {
            if (group.Thumbs.Remove(thumb))
            {
                thumb.DragCompleted -= Thumb_DragCompleted;
                thumb.DragDelta -= Thumb_DragDelta;
                thumb.DragStarted -= Thumb_DragStarted;
                group.TTGroupUpdateLayout();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveAll()
        {
            Thumbs.Clear();
            SelectedThumbs.Clear();
            Data.Datas.Clear();
            ActiveThumb = null;
            ClickedThumb = null;
            ActiveGroup = this;
            TTGroupUpdateLayout();
        }
        #endregion 追加と削除

        #region グループ化

        //基本的にSelectedThumbsの要素群でグループ化、それをActiveGroupに追加する
        public void AddGroup()
        {
            if (SelectedThumbs.Count < 2) { return; }
            TTGroup? group = MakeAndAddGroup(SelectedThumbs, ActiveGroup);
            if (group != null)
            {
                SelectedThumbs.Clear();
                SelectedThumbs.Add(group);
                ActiveThumb = group;
            }
        }
        /// <summary>
        /// グループ化
        /// </summary>
        /// <param name="thumbs">グループ化する要素群</param>
        /// <param name="destGroup">新グループの追加先</param>
        private TTGroup? MakeAndAddGroup(ObservableCollection<TThumb> thumbs, TTGroup destGroup)
        {
            //選択要素群をActiveGroupを基準に並べ替え
            List<TThumb> sortedList = MakeSortedList(thumbs, destGroup);

            //新グループの挿入Index、[^1]は末尾から数えて1番目の要素って意味
            int insertIndex = destGroup.Thumbs.IndexOf(sortedList[^1]) - (sortedList.Count - 1);

            if (CheckAddGroup(sortedList, destGroup) == false) { return null; }
            var (x, y, w, h) = GetRect(sortedList);
            TTGroup newGroup = new()
            {
                TTLeft = x,
                TTTop = y,
                TTGrid = destGroup.TTGrid,
                TTXShift = destGroup.TTXShift,
                TTYShift = destGroup.TTYShift,
            };

            //各要素のドラッグイベントを外す、新グループに追加
            foreach (var item in sortedList)
            {
                if (destGroup.Thumbs.Remove(item) && destGroup.Data.Datas.Remove(item.Data))
                {
                    thumbs.Remove(item);

                    item.DragDelta -= Thumb_DragDelta;
                    item.DragCompleted -= Thumb_DragCompleted;
                    item.DragStarted -= Thumb_DragStarted;

                    newGroup.Thumbs.Add(item);
                    newGroup.Data.Datas.Add(item.Data);
                    item.TTLeft -= x;
                    item.TTTop -= y;
                }
            }

            AddThumb(newGroup, destGroup, insertIndex);
            newGroup.Arrange(new(0, 0, w, h));//再配置？このタイミングで必須、Actualサイズに値が入る
            //↓はこのタイミングではいらないかも？RenderSizeChangeで実行するようにした
            //→要る！！！ここじゃないと枠表示のサイズがなぜか0x0のままになる
            newGroup.TTGroupUpdateLayout();//必須、サイズと位置の更新

            return newGroup;
        }
        //  private TTGroup? MakeAndAddGroup(IEnumerable<TThumb> thumbs, TTGroup destGroup)
        //{
        //    //選択要素群をActiveGroupを基準に並べ替え
        //    List<TThumb> sortedList = MakeSortedList(thumbs, destGroup);

        //    //新グループの挿入Index、[^1]は末尾から数えて1番目の要素って意味
        //    int insertIndex = destGroup.Thumbs.IndexOf(sortedList[^1]) - (sortedList.Count - 1);

        //    if (CheckAddGroup(sortedList, destGroup) == false) { return null; }
        //    var (x, y, w, h) = GetRect(sortedList);
        //    TTGroup newGroup = new()
        //    {
        //        TTLeft = x,
        //        TTTop = y,
        //        TTGrid = destGroup.TTGrid,
        //        TTXShift = destGroup.TTXShift,
        //        TTYShift = destGroup.TTYShift,
        //    };
        //    AddThumb(newGroup, destGroup, insertIndex);

        //    //各要素のドラッグイベントを外す、新グループに追加
        //    foreach (var item in sortedList)
        //    {
        //        destGroup.Thumbs.Remove(item);
        //        destGroup.Data.Datas.Remove(item.Data);
        //        item.DragDelta -= Thumb_DragDelta;
        //        item.DragCompleted -= Thumb_DragCompleted;
        //        item.DragStarted -= Thumb_DragStarted;

        //        newGroup.Thumbs.Add(item);
        //        newGroup.Data.Datas.Add(item.Data);
        //        item.TTLeft -= x;
        //        item.TTTop -= y;
        //    }

        //    newGroup.Arrange(new(0, 0, w, h));//再配置？このタイミングで必須、Actualサイズに値が入る
        //    //↓はこのタイミングではいらないかも？RenderSizeChangeで実行するようにした
        //    //→要る！！！ここじゃないと枠表示のサイズがなぜか0x0のままになる
        //    newGroup.TTGroupUpdateLayout();//必須、サイズと位置の更新

        //    return newGroup;
        //}


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
            if (thumbs.Count() == destGroup.Thumbs.Count)
            {
                if (destGroup.Type == TType.Root) { return true; }
                else { return false; }
            }
            foreach (TThumb thumb in thumbs)
            {
                if (destGroup.Thumbs.Contains(thumb) == false) { return false; }
            }
            return true;
        }
        #endregion グループ化
        #region グループ解除
        /// <summary>
        /// ActiveThumbのグループ解除
        /// </summary>
        public void UnGroup()
        {
            if (ActiveThumb is TTGroup group)
            {
                UnGroup(group, ActiveGroup);
                SelectedThumbs.Clear();
                ActiveThumb = GetActiveThumb(ClickedThumb);
            }
        }
        /// <summary>
        /// 指定グループのグループ解除
        /// </summary>
        /// <param name="group">解除するグループを指定</param>
        /// <param name="destGroup">指定グループの親Group</param>
        public void UnGroup(TTGroup group, TTGroup destGroup)
        {
            //解除対象のグループの要素群に対して
            //ドラッグイベント解除＋解除対象から削除してから
            //その親Groupに追加＋ドラッグイベント追加+位置修正
            int insertIndex = destGroup.Thumbs.IndexOf(group);//挿入Index
            foreach (var item in group.Thumbs.ToArray())
            {
                group.Thumbs.Remove(item);//親Groupから削除
                item.DragDelta -= Thumb_DragDelta;
                item.DragCompleted -= Thumb_DragCompleted;
                item.DragStarted -= Thumb_DragStarted;
                //親親Groupに挿入
                AddThumb(item, destGroup, insertIndex);
                insertIndex++;
                item.TTLeft += group.TTLeft;//位置修正
                item.TTTop += group.TTTop;
            }
            //抜け殻になった元のグループ要素削除
            destGroup.Thumbs.Remove(group);
            group.DragCompleted -= Thumb_DragCompleted;//いる？
            group.DragDelta -= Thumb_DragDelta;
            group.DragStarted -= Thumb_DragStarted;
        }
        #endregion グループ解除

        #region InOut、ActiveThumbの切り替え
        //ActiveThumbを内側(ActiveThumbの親)へ切り替える
        public void ActiveGroupInside()
        {
            if (ActiveThumb is TTGroup group)
            {
                ActiveGroup = group;
                ActiveThumb = GetActiveThumb(ClickedThumb);
                SelectedThumbs.Clear();
            }
        }

        //ActiveThumbを外側(親)へ切り替える
        public void ActiveGroupOutside()
        {
            if (ActiveGroup.TTParent is TTGroup parent)
            {
                ActiveGroup = parent;
                ActiveThumb = GetActiveThumb(ClickedThumb);
                SelectedThumbs.Clear();
            }
        }
        #endregion InOut、ActiveThumbの切り替え

        #region ZIndex
        //ZIndexが同じ場合はThumbsIndexが前後関係になるのを利用して
        //Thumbs要素の入れ替えによって前面、背面移動させる
        #region 背面に移動


        /// <summary>
        /// 選択Thumbを最背面へ移動
        /// </summary>
        /// <returns></returns>
        public bool ZDownBackMost()
        {
            return ZDownBackMost(SelectedThumbs, ActiveGroup);
        }
        /// <summary>
        /// 指定Thumbを最背面へ移動
        /// </summary>
        /// <param name="thumbs">移動させるThumb群</param>
        /// <param name="group">Thumb群の親Group</param>
        /// <returns></returns>
        public bool ZDownBackMost(IEnumerable<TThumb> thumbs, TTGroup group)
        {
            if (IsAllContains(thumbs, group) == false) { return false; }
            //下側にある要素から処理したいので、並べ替えたListを作成
            List<TThumb> tempList = MakeSortedList(thumbs, group);
            //削除してから先頭から挿入
            for (int i = 0; i < tempList.Count; i++)
            {
                if (group.Thumbs.Remove(tempList[i]))
                {
                    group.Thumbs.Insert(i, tempList[i]);
                }
                else { return false; }
            }
            return true;
        }
        /// <summary>
        /// 選択Thumbを背面へ移動
        /// </summary>
        /// <returns></returns>
        public bool ZDown()
        {
            return ZDown(SelectedThumbs, ActiveGroup);
        }
        /// <summary>
        /// 指定Thumbを背面へ移動
        /// </summary>
        /// <param name="thumbs"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool ZDown(IEnumerable<TThumb> thumbs, TTGroup group)
        {
            if (IsAllContains(thumbs, group) == false) { return false; }
            //順番を揃えたリスト作成
            List<TThumb> tempList = MakeSortedList(thumbs, group);

            //一番下の要素がもともと一番下だった場合は処理しない
            if (group.Thumbs[0] == tempList[0]) { return false; }

            //順番に処理、削除してから挿入、挿入箇所は元のインデックス-1
            foreach (var item in tempList)
            {
                int ii = group.Thumbs.IndexOf(item);
                ii--;
                if (group.Thumbs.Remove(item))
                {
                    group.Thumbs.Insert(ii, item);
                }
                else
                {
                    return false;
                    throw new ArgumentException("対象要素が親要素から見つからなかった");
                }
            }

            return true;
        }
        #endregion 背面に移動

        #region 前面に移動

        /// <summary>
        /// 指定Thumbを最前面へ移動
        /// </summary>
        /// <param name="thumbs">移動させるThumb群</param>
        /// <param name="group">親Group</param>
        /// <returns></returns>
        public bool ZUpFrontMost(IEnumerable<TThumb> thumbs, TTGroup group)
        {
            //要素すべてがGroupのChildrenに存在するか判定、存在しない要素があれば処理しない            
            if (IsAllContains(thumbs, group) == false) { return false; }

            //下側にある要素から処理したいので、並べ替えたListを作成
            List<TThumb> tempList = MakeSortedList(thumbs, group);
            //削除してから追加(末尾に追加)
            foreach (var item in tempList)
            {
                if (group.Thumbs.Remove(item))
                {
                    group.Thumbs.Add(item);
                }
                else { return false; }
            }
            return true;
        }
        /// <summary>
        /// 選択Thumbを最前面へ移動
        /// </summary>
        /// <returns></returns>
        public bool ZUpFrontMost()
        {
            return ZUpFrontMost(SelectedThumbs, ActiveGroup);
        }

        /// <summary>
        /// 指定Thumbを前面へ移動
        /// </summary>
        /// <param name="thumbs"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool ZUp(IEnumerable<TThumb> thumbs, TTGroup group)
        {
            if (IsAllContains(thumbs, group) == false) { return false; }
            //順番を揃えてから削除して追加
            List<TThumb> tempList = MakeSortedList(thumbs, group);

            //一番上の要素がもともと一番上だった場合は処理しない
            if (group.Thumbs[^1] == tempList[^1])
            {
                return false;
            }
            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                int ii = group.Thumbs.IndexOf(tempList[i]);
                ii++;
                if (group.Thumbs.Remove(tempList[i]))
                {
                    group.Thumbs.Insert(ii, tempList[i]);
                }
                else { return false; }
            }
            return true;

        }
        /// <summary>
        /// 選択Thumbを前面へ移動
        /// </summary>
        /// <returns></returns>
        public bool ZUp()
        {
            return ZUp(SelectedThumbs, ActiveGroup);
        }
        #endregion 前面に移動


        //backmost 最背面 back to back最背面にする
        //frontmsot 一番前
        //front 最前面
        //put one back  一つ後ろにする
        #endregion ZIndex

        #region 画像として取得
        /// <summary>
        /// 指定Thumbを画像として取得
        /// </summary>
        /// <param name="thumb">画像として取得したいThumb</param>
        /// <returns></returns>
        public BitmapSource? GetBitmap(TThumb thumb)
        {
            if (thumb.Type != TType.Root)
            {
                return SaveImage2(thumb, thumb.TTParent);
            }
            else
            {
                return SaveImage2(thumb, thumb);
            }
        }
        private BitmapSource? SaveImage2(FrameworkElement? el, FrameworkElement? parentPanel)
        {
            if (el == null || parentPanel == null) { return null; }
            GeneralTransform gt = el.TransformToVisual(parentPanel);
            Rect bounds = gt.TransformBounds(new Rect(0, 0, el.ActualWidth, el.ActualHeight));
            DrawingVisual dVisual = new();
            //var debounds = VisualTreeHelper.GetDescendantBounds(parentPanel);
            //四捨五入しているけど、UselayoutRoundingをtrueにしていたら必要なさそう
            bounds.Width = (int)(bounds.Width + 0.5);
            bounds.Height = (int)(bounds.Height + 0.5);

            using (DrawingContext context = dVisual.RenderOpen())
            {
                VisualBrush vBrush = new(el) { Stretch = Stretch.None };
                //context.DrawRectangle(vBrush, null, new Rect(0, 0, bounds.Width, bounds.Height));
                //context.DrawRectangle(vBrush, null, bounds);

                context.DrawRectangle(vBrush, null, new Rect(bounds.Size));
            }
            RenderTargetBitmap bitmap
                = new((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dVisual);

            return bitmap;
        }
        #endregion 画像として取得

    }




    public class TTTextBlock : TThumb
    {
        #region 依存プロパティ

        public string TTText
        {
            get { return (string)GetValue(TTTextProperty); }
            set { SetValue(TTTextProperty, value); }
        }
        public static readonly DependencyProperty TTTextProperty =
            DependencyProperty.Register(nameof(TTText), typeof(string), typeof(TTTextBlock),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 依存プロパティ


        private readonly TextBlock MyTemplateElement;

        public TTTextBlock() : this(new Data(TType.TextBlock)) { }

        public TTTextBlock(Data data) : base(data)
        {
            Data = data;
            this.DataContext = Data;
            if (MakeTemplate<TextBlock>() is TextBlock element)
            {
                MyTemplateElement = element;
            }
            else { throw new ArgumentException("テンプレート作成できんかった"); }

            SetBinding(TTTextProperty, nameof(Data.Text));
            MyTemplateElement.SetBinding(TextBlock.TextProperty, nameof(Data.Text));
            //MySetXYBinging(this.Data);
        }
    }


    public class TTImage : TThumb
    {

        public string TTSourcePath
        {
            get { return (string)GetValue(TTSourcePathProperty); }
            set { SetValue(TTSourcePathProperty, value); }
        }
        public static readonly DependencyProperty TTSourcePathProperty =
            DependencyProperty.Register(nameof(TTSourcePath), typeof(string), typeof(TTImage),
                new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnTTUriChanged)));

        private static void OnTTUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TTImage obj)
            {
                obj.Data.BitmapSource = new BitmapImage(new Uri((string)e.NewValue));
            }
        }

        private readonly Image MyTemplateElement;


        public TTImage() : this(new Data(TType.Image)) { }
        public TTImage(Data data) : base(data)
        {
            Data = data;
            this.DataContext = Data;
            if (MakeTemplate<Image>() is Image element) { MyTemplateElement = element; }
            else { throw new ArgumentException("テンプレート作成できんかった"); }

            //SetBinding(TTSourceProperty, nameof(Data.BitmapSource));
            MyTemplateElement.SetBinding(Image.SourceProperty, nameof(Data.BitmapSource));
            //MySetXYBinging(this.Data);
        }

    }



}