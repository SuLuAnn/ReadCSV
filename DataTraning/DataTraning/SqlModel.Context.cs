﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataTraning
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class StockDBEntities : DbContext
    {
        public StockDBEntities()
            : base("name=StockDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<日期貨盤後行情表_luann> 日期貨盤後行情表_luann { get; set; }
        public virtual DbSet<日期貨盤後統計表_luann> 日期貨盤後統計表_luann { get; set; }
        public virtual DbSet<股東會投票日明細_luann> 股東會投票日明細_luann { get; set; }
        public virtual DbSet<股東會投票資料表_luann> 股東會投票資料表_luann { get; set; }
        public virtual DbSet<基金非營業日明細_luann> 基金非營業日明細_luann { get; set; }
        public virtual DbSet<基金非營業日統計_luann> 基金非營業日統計_luann { get; set; }
    }
}