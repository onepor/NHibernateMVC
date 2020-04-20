﻿using Castle.ActiveRecord;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 模拟树绑定
    /// </summary> 
    public partial class JQueryFeature
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _level;

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        private bool _enableSelect;

        public bool EnableSelect
        {
            get { return _enableSelect; }
            set { _enableSelect = value; }
        }

        public JQueryFeature(string id, string name, int level, bool enableSelect)
        {
            _id = id;
            _name = name;
            _level = level;
            _enableSelect = enableSelect;
        }
         
        public override string ToString()
        {
            return string.Format("Name:{0}+Id:{1}", Name, Id);
        }
    }
}

