﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using System.Data;
using WPF.ListViewDragDrop;//listview拖拽相关
using System.Windows.Threading;
using System.Threading;



namespace LoveMusic
{
 
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public enum goPlay
        {
            下一首,
            上一首,
            双击列表,
            继续暂停,
            正常播放
        }
        public enum playMode
        {
            循环播放,
            单曲循环,
            随机播放
        }

        #region 涉及到的变量
        //建立一个集合用来存放当前列表所有音乐文件
        //ObservableCollection类 表示一个动态数据集合，在添加项、移除项或刷新整个列表时，此集合将提供通知。用于集合对象数据绑定
        ObservableCollection<Music> music = new ObservableCollection<Music>();
        ListViewDragDropManager<Music> dragMgr;        
        Time t = new Time();
        TimeSpan time = new TimeSpan();                      //音乐文件播放时间
        public static int playState = -1;                    //当前播放状态 -1没有播放 0播放 1暂停
        public static  int buttonState{set;get;}             //当前播放按钮状态 0 按钮样式：播放，  1 按钮样式：暂停
        static playMode p;                                   //当前播放模式
        static goPlay cb;                                    //下一步执行
        static int num=0;                                    //随机数索引
        static List<int> randomNum = new List<int>();        //存放随机播放时产生的随机数
        static int count = 0;                                //存放音乐文件总数
        static int playNum = -1;                              //播放歌曲索引
        static bool lrcOpen=false;
        static MediaPlayer player = new MediaPlayer();
        static Lrc lrc = new Lrc();
        DispatcherTimer timer = new DispatcherTimer();
        #endregion
        /// <summary>
        /// 获取音乐文件总数
        /// </summary>     
        public void madePlayNum()
        {
            #region 产生不重复随机数
                //产生不重复的随机数
                int item;
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                for (int i = 0; i < count; i++)
                {
                    //产生>=0，小于count的随机数
                    item = rand.Next(0, count);
                    while (randomNum.IndexOf(item) != -1) item = rand.Next(0, count);
                    randomNum.Add(item);
                }                
            #endregion
        }


        public void playMusic(goPlay cb)
        {
            if (cb == goPlay.下一首)
            {
                if (count<=0) return;
                if (p == playMode.循环播放)
                {
                    if (playNum < count - 1) playNum++;
                    else if (playNum >= count - 1) playNum = 0;
                }
                else if (p == playMode.随机播放)
                {
                    num++;
                    if (randomNum.Count == 0) madePlayNum();
                    if (num == count) randomNum.Clear();
                    playNum = randomNum[num];
                }
                cb = goPlay.正常播放;
            }
            else if (cb == goPlay.上一首)
            {
                if (count<=0) return;
                if (p==playMode.循环播放)
                {
                    if (playNum > 0) playNum--;
                    else if (playNum <= 0) playNum = count - 1;
                }
                else if (p == playMode.随机播放)
                {
                    num--;
                    if (randomNum.Count == 0) madePlayNum();
                    if (playNum == count) randomNum.Clear();
                    playNum = randomNum[num];
                }
                cb = goPlay.正常播放;
            }
            //程序在播放歌曲，按钮处于暂停样式,按下后执行暂停播放
            else if ((buttonState == 1) && (playState == 0) && cb == goPlay.继续暂停)
            {
                //播放状态playState -1:停止 0:播放 1:暂停    buttonState按钮状态 0:播放 1:暂停
                buttonState = 0;
                playState = 1;
                //程序暂停放歌，按钮处于播放样式
                cb = goPlay.继续暂停;
                btPlay.Style = (Style)this.FindResource("ButtonStylePlay");
                player.Pause();
            }
            //程序在暂停播放，按钮处于播放样式,按下后继续播放.这里else不要漏掉
            else if ((buttonState == 0) && (playState == 1) && cb == goPlay.继续暂停)
            {
                buttonState = 1;
                playState = 0;
                btPlay.Style = (Style)this.FindResource("ButtonStylePause");
                cb = goPlay.继续暂停;
                player.Play();
            }
            //通过列表双击播放
            else if (cb == goPlay.双击列表)
            {
                playNum = (int)musicList.SelectedIndex;
                if (playNum == -1) return;
                cb = goPlay.正常播放;
            }
            if (cb == goPlay.正常播放)
            {
                if (p == playMode.单曲循环) player.Play();
                player.Close();
                buttonState = 1;
                playState = 0;
                player.Open(new Uri(music[playNum].musicPath, UriKind.Relative));
                btPlay.Style = (Style)this.FindResource("ButtonStylePause");
                player.Play();
                Lrc.Clear(txtb1,lrc1);
                Lrc.musicName = music[playNum].musicName;
                lrc.LoadLrc(lrc1);
                cb = goPlay.继续暂停;
            }
                musicList.SelectedIndex = playNum;
                musicList.ScrollIntoView(musicList.SelectedItem);
        }


        public MainWindow()
        {
            InitializeComponent();
            //获取或设置用于生成 ItemsControl 的内容的集合。即数据绑定到集合music
            musicList.ItemsSource = music;
            //涉及到的数据绑定
            musicList.DisplayMemberPath = "musicName";
            sdMusic.DataContext = t;
            txtSec.DataContext = t;
            txtMin.DataContext = t;
            p = playMode.循环播放;
        }
        /// <summary>
        /// 定时器回调函数
        /// </summary>
        void timer_Tick(object sender, object e)
        {
            
            time = player.Position;
            t.AllTime =(int) time.TotalSeconds;
            t.Seconds = time.Seconds;
            t.Minutes = time.Minutes;
            if (playNum != -1 && lrcOpen==true)
            {
                lrc.ShowLrc(t.Minutes, t.Seconds,txtb1,lrc1);
            }
            if (t.AllTime == sdMusic.Maximum &&t.AllTime!=0)
            {
                cb = goPlay.下一首;
                
                playMusic(cb);
                time = player.Position;
                t.AllTime = (int)time.TotalSeconds;
                t.Seconds = time.Seconds;
                t.Minutes = time.Minutes;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //listview拖拽实现
            this.dragMgr = new ListViewDragDropManager<Music>(this.musicList);
            if (File.Exists("默认.lst") == true)
            {
                StreamReader sr = new StreamReader("默认.lst");
                string st = "";
                while (sr.EndOfStream == false)
                {
                    Music tmp = new Music();
                    st = sr.ReadLine();
                    tmp.musicPath = st;
                    tmp.musicName = Music.GetMusicName(st);
                    if ((tmp.musicPath != null) && (tmp.musicName != "?"))
                    {
                        music.Add(tmp);
                    }
                }
                sr.Close();
                //获取列表文件数目
                count = music.Count;
            }
            //媒体打开时触发方法player_MediaOpened
            player.MediaOpened += new EventHandler(player_MediaOpened);
            //设置定时器同步播放进度
            //获取或设置计时器刻度之间的时间段。 
            timer.Interval = TimeSpan.FromMilliseconds(500);	
            
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void btOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "所有音频文件|*.mp3;*.wma;*.mid;*.asf;*.flac;*.wmv;*.wm;*.rm;*.mp4;*.mpg;*.mpeg;*.m1v;*.mp2;*.mpa;*.mpe;*.mpv2;*.m3u;*.wav;*.cda";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == true)
            {
                foreach (string FileName in ofd.FileNames)
                {
                    //创建一个临时音乐类用来写入音乐类集合music
                    Music temp = new Music();
                    //创建一个对象用于处理出现关于字符串的问题 
                    //去除FileName的目录路径，保留文件名及后缀
                    temp.musicPath = FileName;
                    temp.musicName = Music.GetMusicName(FileName);
                    //将temp写入集合
                    music.Add(temp);
                }
            }
        }

        private void btPlay_Click(object sender, RoutedEventArgs e)
        {
            if(playState!=-1)cb = goPlay.继续暂停;
            if(playState==-1) cb = goPlay.双击列表;
            playMusic(cb);
        }


        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            
            cb = goPlay.下一首;
            playMusic(cb);
        }


        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            player.Close();
            Music.WriteMusicList("默认", music); 
            music.Clear();
            Close();
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            cb = goPlay.上一首;
            playMusic(cb);
        }

        //获取音频长度
        //设置时间轴总长度。由于NaturalDuration为Automatic对外部拒绝访问不能直接获取
        private void player_MediaOpened(object sender, EventArgs e)
        {
            try
            {
               sdMusic.Maximum = (int)player.NaturalDuration.TimeSpan.TotalSeconds;
            }
            catch
            {
                
                MessageBox.Show("你操作太快了。") ;
            }
        }

        //当播放进度数值发生变化时，把歌曲播放位置设定到触发点
        private void sdMusic_ViewChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //变化绝对值大于1时才触发，避免和定时器冲突
            if ((e.NewValue - e.OldValue > 1)||(e.NewValue-e.OldValue<1))
            {
                if (playState == -1) t.AllTime = 0;
                player.Position = new TimeSpan((int)sdMusic.Value / 3600, (int)sdMusic.Value / 60, (int)sdMusic.Value % 60);
            }
        }

        private void musicList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            cb = goPlay.双击列表;
            playMusic(cb);
        }

        private void btLrc_Click(object sender, RoutedEventArgs e)
        {
            
            //Thread NetServer = new Thread(new ThreadStart(ThreadProc));
            //NetServer.SetApartmentState(ApartmentState.STA);
            //NetServer.Start();
            if (lrcOpen == false)
            {
                btLrc.Style = (Style)this.FindResource("ButtonStyleLrcOpen");
                lrcOpen = true;
                tb1.Visibility = Visibility.Visible;
                lr1.Visibility = Visibility.Visible;
                txtb1.Visibility = Visibility.Visible;
                lrc1.Visibility = Visibility.Visible;
            }
            else
            { 
                btLrc.Style = (Style)this.FindResource("ButtonStyleLrcClose");
                lrcOpen = false;
                tb1.Visibility = Visibility.Collapsed;
                lr1.Visibility = Visibility.Collapsed;
                txtb1.Visibility = Visibility.Collapsed;
                lrc1.Visibility = Visibility.Collapsed;

            }
        }
    }
}
