﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FightTheLandLord
{
    public partial class MainForm : Form
    {
        private List<Poker> allPoker = new List<Poker>();
        private Player player1 = new Player();
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        public void shuffle()
        {
            Poker lastPoker;
            try
            {
                for(int i =0;i < 5000;i++)  //洗牌,六个随机数向下替换.
                {
                    int num1 = new Random().Next(0, 27);
                    int num2 = new Random().Next(28, 54);
                    int num3 = new Random().Next(0, 54);
                    int num4 = new Random().Next(0, 10);
                    int num5 = new Random().Next(34, 54);
                    int num6 = new Random().Next(45, 54);
                    lastPoker = this.allPoker[num1];
                    this.allPoker[num1] = this.allPoker[num2];
                    this.allPoker[num2] = this.allPoker[num3];
                    this.allPoker[num3] = this.allPoker[num4];
                    this.allPoker[num4] = this.allPoker[num5];
                    this.allPoker[num5] = this.allPoker[num6];
                    this.allPoker[num6] = lastPoker;
                }
#if DEBUG
                Console.WriteLine("以下是洗过的牌");
                foreach (Poker onePoker in this.allPoker)
                {
                    Console.WriteLine(onePoker.pokerColor.ToString() + onePoker.pokerNum.ToString());
                }
#endif
            }
            catch
            {
            }
        }

        /// <summary>
        /// 发牌
        /// </summary>
        public void deal()
        {
            for (int i = 0; i < 17; i++)
            {
                player1.pokers.Add(this.allPoker[i]);
            }

#if DEBUG
            Console.WriteLine("玩家一的牌");
            foreach (Poker onePoker in player1.pokers)
            {
                Console.WriteLine(onePoker.pokerColor.ToString() + onePoker.pokerNum.ToString());
            }
#endif
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            for (int i = 3; i < 18; i++)  //嵌套for循环初始化54张牌
            {
                for (int j = 1; j < 5; j++)
                {
                    if (i <= 15)
                    {
                        this.allPoker.Add(new Poker((PokerNum)i, (PokerColor)j));
                    }
                }
                if (i >= 16)
                {
                    this.allPoker.Add(new Poker((PokerNum)i, PokerColor.黑桃));
                }
            }                           //嵌套for循环初始化54张牌

#if DEBUG
            Console.WriteLine(allPoker.Count);
            foreach (Poker onePoker in this.allPoker)
            {
                Console.WriteLine(onePoker.pokerColor.ToString() + onePoker.pokerNum.ToString());
            }
#endif
            shuffle(); //洗牌
            deal(); //发牌
            this.player1.sort(); //把牌从大到小排序
            this.player1.g = this.panelPlayer1.CreateGraphics(); //把panelPlayer1的Graphics传递给player1
            this.player1.Paint(); //在panelPlayer1中画出player1的牌
            this.panelPlayer1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelPlayer1_MouseClick); //给panelPlayer1添加一个点击事件
            this.btnStart.Enabled = false;
            this.btnStart.Visible = false;
            this.btnLead.Enabled = true;
            this.btnLead.Visible = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void panelPlayer1_Paint(object sender, PaintEventArgs e)
        {
            this.player1.Paint();
        }

        private void panelPlayer1_MouseClick(object sender, MouseEventArgs e)  //鼠标点击事件,处理选中牌的效果
        {
            int index;
            this.player1.backColor = this.panelPlayer1.BackColor;
            if ((int)(e.X / 40) < player1.newPokers.Count)
            {
                index = (int)(e.X / 40);
                this.player1.Paint(index);
            }
        }

        private void btnLead_Click(object sender, EventArgs e)
        {
            if (player1.lead())
            {
                this.lblIsRule.Text = "";
                player1.g.Clear(this.BackColor);
                player1.Paint();
            }
            else
            {
                this.lblIsRule.ForeColor = Color.Red;
                this.lblIsRule.Text = "您出的牌不符合规则";

            }
        }

        private void 创建游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Server server = new Server();
            server.listener.Start();
            server.Connection();
        }

        private void 加入游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JoinForm joinForm = new JoinForm();
            joinForm.ShowDialog();
        }
    }
}
