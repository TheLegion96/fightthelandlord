﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace MapEditor
{
    [Serializable]
    public class Project
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 所在目录
        /// </summary>
        [NonSerialized]
        public string Directory;

        /// <summary>
        /// 所有地图
        /// </summary>
        private List<Map> _allMaps = new List<Map>();

        /// <summary>
        /// 所有地图
        /// </summary>
        public List<Map> AllMaps
        {
            get
            {
                return this._allMaps;
            }
            set
            {
                this._allMaps = value;
            }
        }

        /// <summary>
        /// 全局开关
        /// </summary>
        private List<OnOff> _globalOnOff = new List<OnOff>();

        /// <summary>
        /// 全局开关
        /// </summary>
        public List<OnOff> globalOnOff
        {
            get
            {
                return this._globalOnOff;
            }
            set
            {
                this._globalOnOff = value;
            }
        }
        /// <summary>
        /// 通过开关名获取开关
        /// </summary>
        /// <param name="name">开关名称</param>
        /// <returns>选中的开关</returns>
        public OnOff GetGlobalOnOffByName(string name)
        {
            OnOff selectedOnOff = null;
            foreach (var onOff in globalOnOff)
            {
                if (onOff.Name == name)
                    selectedOnOff = onOff;
            }
            return selectedOnOff;
        }

        /// <summary>
        /// 设置指定开关名的值
        /// </summary>
        /// <param name="name">开关名称</param>
        /// <param name="value">开关值</param>
        public void SetGlobalOnOff(string name, bool value)
        {
            foreach (var onOff in globalOnOff)
            {
                if (onOff.Name == name)
                {
                    onOff.Value = value;
                }
            }
        }
        /// <summary>
        /// 添加地图
        /// </summary>
        /// <param name="map">地图</param>
        public void AddMap(Map map)
        {
            this.AllMaps.Add(map);
        }

        public Map GetMapByName(string name)
        {
            foreach (var singleMap in AllMaps)
            {
                if (singleMap.Name == name)
                    return singleMap;
            }
            return null;
        }

        public void InitMap()
        {
            foreach (var singleMap in AllMaps)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(this.Directory + "Map\\" + singleMap.ImageFileName));
                var image = new Image() { Source = bitmap };
                singleMap.MapImage = image;
            }
        }
    }
}
