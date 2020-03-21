﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sprite.Common.Finder
{
    /// <summary>
    /// 查找器基类
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public abstract class FinderBase<TItem> : IFinder<TItem>
    {
        private readonly object _lockObj = new object();
        /// <summary>
        /// 项缓存
        /// </summary>
        protected readonly List<TItem> ItemsCache = new List<TItem>();
        /// <summary>
        /// 是否已查找过
        /// </summary>
        protected bool Found = false;
        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <param name="fromCache">是否读取缓存</param>
        /// <returns></returns>
        public TItem[] Find(Func<TItem, bool> predicate, bool fromCache = false)
        {
            return FindAll(fromCache).Where(predicate).ToArray();
        }
        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public TItem[] FindAll(bool fromCache = false)
        {
            lock (_lockObj)
            {
                if (Found && fromCache)
                {
                    return ItemsCache.ToArray();
                }

                TItem[] items = FindAllItems();
                Found = true;
                ItemsCache.Clear();
                ItemsCache.AddRange(items);
                return items;
            }
        }
        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected abstract TItem[] FindAllItems();
    }
}
