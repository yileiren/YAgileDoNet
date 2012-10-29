using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YLR.YAdoNet
{
    /// <summary>
    /// 分页数据，用来提供分页查询时返回的数据。
    /// 作者：董帅 创建时间：2012-10-29 13:01:28
    /// </summary>
    public class PagerData
    {
        /// <summary>
        /// 指定页的数据。
        /// </summary>
        protected DataTable _data = null;

        /// <summary>
        /// 指定页的数据。
        /// </summary>
        public DataTable data
        {
            get { return this._data; }
            set { this._data = value; }
        }

        /// <summary>
        /// 总页数。
        /// </summary>
        protected int _pageCount = 1;

        /// <summary>
        /// 总页数。
        /// </summary>
        public int pageCount
        {
            get { return this._pageCount; }
            set { this._pageCount = value; }
        }

        /// <summary>
        /// 当前页号，从1开始。
        /// </summary>
        protected int _pageNum = 1;

        /// <summary>
        /// 当前页号，从1开始。
        /// </summary>
        public int pageNum
        {
            get { return this._pageNum; }
            set { this._pageNum = value; }
        }

        /// <summary>
        /// 每页显示的数据数量。
        /// </summary>
        protected int _dataCount = 20;

        /// <summary>
        /// 每页显示的数据数量。
        /// </summary>
        public int dataCount
        {
            get { return this._dataCount; }
            set { this._dataCount = value; }
        }
    }
}
