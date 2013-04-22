using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YLR.YAdoNet
{
    /// <summary>
    /// 参数列表类，构建command中使用的Parameter的名称和数值，与Parameter的用法一样。
    /// </summary>
    public class YParameters
    {
        /// <summary>
        /// 参数列表。
        /// </summary>
        private List<YParameterValue> parameters = new List<YParameterValue>();

        /// <summary>
        /// 在列表末尾增加参数。
        /// </summary>
        /// <param name="name">参数名称。</param>
        /// <param name="value">数据。</param>
        public void add(string name, object value)
        {
            this.parameters.Add(new YParameterValue(name, value));
        }

        /// <summary>
        /// 清空参数列表中的数据。
        /// </summary>
        public void clear()
        {
            this.parameters.Clear();
        }

        /// <summary>
        /// 参数数量。
        /// </summary>
        public int Count
        {
            get { return this.parameters.Count; }
        }

        /// <summary>
        /// 获取指定位置的名称。
        /// </summary>
        /// <param name="p">位置索引。</param>
        /// <returns>名称</returns>
        public string getName(int p)
        {
            return this.parameters[p].name;
        }

        /// <summary>
        /// 获取指定位置的数据。
        /// </summary>
        /// <param name="p">位置索引。</param>
        /// <returns>数据。</returns>
        public object getValue(int p)
        {
            return this.parameters[p].value;
        }

        /// <summary>
        /// 参数类。
        /// </summary>
        private class YParameterValue
        {
            /// <summary>
            /// 默认构造函数。
            /// </summary>
            public YParameterValue()
            {
 
            }

            /// <summary>
            /// 够咱函数。
            /// </summary>
            /// <param name="name">参数名称。</param>
            /// <param name="value">参数值。</param>
            public YParameterValue(string name, object value)
            {
                this.name = name;
                this.value = value;
            }

            /// <summary>
            /// 参数名称。
            /// </summary>
            private string _name = "";

            /// <summary>
            /// 参数名称。
            /// </summary>
            public string name
            {
                get { return this._name; }
                set { this._name = value; }
            }

            /// <summary>
            /// 参数值。
            /// </summary>
            private object _value = null;

            /// <summary>
            /// 参数值。
            /// </summary>
            public object value
            {
                get { return this._value; }
                set { _value = value; }
            }
        }
    }
}
