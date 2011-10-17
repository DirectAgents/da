﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EomApp1.Formss.Accounting.Data
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="DADatabaseR1")]
	public partial class AccountingDataClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
        //private bool p;
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertItem(Item instance);
    partial void UpdateItem(Item instance);
    partial void DeleteItem(Item instance);
    partial void InsertAffiliate(Affiliate instance);
    partial void UpdateAffiliate(Affiliate instance);
    partial void DeleteAffiliate(Affiliate instance);
    #endregion
		
		public AccountingDataClassesDataContext() : 
				base(global::EomApp1.Properties.Settings.Default.DADatabaseR1ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public AccountingDataClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountingDataClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountingDataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountingDataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

        public AccountingDataClassesDataContext(bool p)
            : base(global::DAgents.Common.Properties.Settings.Default.ConnStr, mappingSource)
        {
            OnCreated();
        }
		
		public System.Data.Linq.Table<Item> Items
		{
			get
			{
				return this.GetTable<Item>();
			}
		}
		
		public System.Data.Linq.Table<Affiliate> Affiliates
		{
			get
			{
				return this.GetTable<Affiliate>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Item")]
	public partial class Item : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _name;
		
		private int _pid;
		
		private int _affid;
		
		private int _source_id;
		
		private int _unit_type_id;
		
		private System.Nullable<int> _stat_id_n;
		
		private int _revenue_currency_id;
		
		private int _cost_currency_id;
		
		private decimal _revenue_per_unit;
		
		private decimal _cost_per_unit;
		
		private decimal _num_units;
		
		private string _notes;
		
		private string _accounting_notes;
		
		private int _item_accounting_status_id;
		
		private int _item_reporting_status_id;
		
		private System.Nullable<decimal> _total_revenue;
		
		private System.Nullable<decimal> _total_cost;
		
		private System.Nullable<decimal> _margin;
		
		private EntityRef<Affiliate> _Affiliate;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void OnpidChanging(int value);
    partial void OnpidChanged();
    partial void OnaffidChanging(int value);
    partial void OnaffidChanged();
    partial void Onsource_idChanging(int value);
    partial void Onsource_idChanged();
    partial void Onunit_type_idChanging(int value);
    partial void Onunit_type_idChanged();
    partial void Onstat_id_nChanging(System.Nullable<int> value);
    partial void Onstat_id_nChanged();
    partial void Onrevenue_currency_idChanging(int value);
    partial void Onrevenue_currency_idChanged();
    partial void Oncost_currency_idChanging(int value);
    partial void Oncost_currency_idChanged();
    partial void Onrevenue_per_unitChanging(decimal value);
    partial void Onrevenue_per_unitChanged();
    partial void Oncost_per_unitChanging(decimal value);
    partial void Oncost_per_unitChanged();
    partial void Onnum_unitsChanging(decimal value);
    partial void Onnum_unitsChanged();
    partial void OnnotesChanging(string value);
    partial void OnnotesChanged();
    partial void Onaccounting_notesChanging(string value);
    partial void Onaccounting_notesChanged();
    partial void Onitem_accounting_status_idChanging(int value);
    partial void Onitem_accounting_status_idChanged();
    partial void Onitem_reporting_status_idChanging(int value);
    partial void Onitem_reporting_status_idChanged();
    partial void Ontotal_revenueChanging(System.Nullable<decimal> value);
    partial void Ontotal_revenueChanged();
    partial void Ontotal_costChanging(System.Nullable<decimal> value);
    partial void Ontotal_costChanged();
    partial void OnmarginChanging(System.Nullable<decimal> value);
    partial void OnmarginChanged();
    #endregion
		
		public Item()
		{
			this._Affiliate = default(EntityRef<Affiliate>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="VarChar(300)")]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_pid", DbType="Int NOT NULL")]
		public int pid
		{
			get
			{
				return this._pid;
			}
			set
			{
				if ((this._pid != value))
				{
					this.OnpidChanging(value);
					this.SendPropertyChanging();
					this._pid = value;
					this.SendPropertyChanged("pid");
					this.OnpidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_affid", DbType="Int NOT NULL")]
		public int affid
		{
			get
			{
				return this._affid;
			}
			set
			{
				if ((this._affid != value))
				{
					if (this._Affiliate.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnaffidChanging(value);
					this.SendPropertyChanging();
					this._affid = value;
					this.SendPropertyChanged("affid");
					this.OnaffidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_source_id", DbType="Int NOT NULL")]
		public int source_id
		{
			get
			{
				return this._source_id;
			}
			set
			{
				if ((this._source_id != value))
				{
					this.Onsource_idChanging(value);
					this.SendPropertyChanging();
					this._source_id = value;
					this.SendPropertyChanged("source_id");
					this.Onsource_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_unit_type_id", DbType="Int NOT NULL")]
		public int unit_type_id
		{
			get
			{
				return this._unit_type_id;
			}
			set
			{
				if ((this._unit_type_id != value))
				{
					this.Onunit_type_idChanging(value);
					this.SendPropertyChanging();
					this._unit_type_id = value;
					this.SendPropertyChanged("unit_type_id");
					this.Onunit_type_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_stat_id_n", DbType="Int")]
		public System.Nullable<int> stat_id_n
		{
			get
			{
				return this._stat_id_n;
			}
			set
			{
				if ((this._stat_id_n != value))
				{
					this.Onstat_id_nChanging(value);
					this.SendPropertyChanging();
					this._stat_id_n = value;
					this.SendPropertyChanged("stat_id_n");
					this.Onstat_id_nChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_revenue_currency_id", DbType="Int NOT NULL")]
		public int revenue_currency_id
		{
			get
			{
				return this._revenue_currency_id;
			}
			set
			{
				if ((this._revenue_currency_id != value))
				{
					this.Onrevenue_currency_idChanging(value);
					this.SendPropertyChanging();
					this._revenue_currency_id = value;
					this.SendPropertyChanged("revenue_currency_id");
					this.Onrevenue_currency_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_cost_currency_id", DbType="Int NOT NULL")]
		public int cost_currency_id
		{
			get
			{
				return this._cost_currency_id;
			}
			set
			{
				if ((this._cost_currency_id != value))
				{
					this.Oncost_currency_idChanging(value);
					this.SendPropertyChanging();
					this._cost_currency_id = value;
					this.SendPropertyChanged("cost_currency_id");
					this.Oncost_currency_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_revenue_per_unit", DbType="Money NOT NULL")]
		public decimal revenue_per_unit
		{
			get
			{
				return this._revenue_per_unit;
			}
			set
			{
				if ((this._revenue_per_unit != value))
				{
					this.Onrevenue_per_unitChanging(value);
					this.SendPropertyChanging();
					this._revenue_per_unit = value;
					this.SendPropertyChanged("revenue_per_unit");
					this.Onrevenue_per_unitChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_cost_per_unit", DbType="Money NOT NULL")]
		public decimal cost_per_unit
		{
			get
			{
				return this._cost_per_unit;
			}
			set
			{
				if ((this._cost_per_unit != value))
				{
					this.Oncost_per_unitChanging(value);
					this.SendPropertyChanging();
					this._cost_per_unit = value;
					this.SendPropertyChanged("cost_per_unit");
					this.Oncost_per_unitChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_num_units", DbType="Decimal(12,6) NOT NULL")]
		public decimal num_units
		{
			get
			{
				return this._num_units;
			}
			set
			{
				if ((this._num_units != value))
				{
					this.Onnum_unitsChanging(value);
					this.SendPropertyChanging();
					this._num_units = value;
					this.SendPropertyChanged("num_units");
					this.Onnum_unitsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_notes", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string notes
		{
			get
			{
				return this._notes;
			}
			set
			{
				if ((this._notes != value))
				{
					this.OnnotesChanging(value);
					this.SendPropertyChanging();
					this._notes = value;
					this.SendPropertyChanged("notes");
					this.OnnotesChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_accounting_notes", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string accounting_notes
		{
			get
			{
				return this._accounting_notes;
			}
			set
			{
				if ((this._accounting_notes != value))
				{
					this.Onaccounting_notesChanging(value);
					this.SendPropertyChanging();
					this._accounting_notes = value;
					this.SendPropertyChanged("accounting_notes");
					this.Onaccounting_notesChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_item_accounting_status_id", DbType="Int NOT NULL")]
		public int item_accounting_status_id
		{
			get
			{
				return this._item_accounting_status_id;
			}
			set
			{
				if ((this._item_accounting_status_id != value))
				{
					this.Onitem_accounting_status_idChanging(value);
					this.SendPropertyChanging();
					this._item_accounting_status_id = value;
					this.SendPropertyChanged("item_accounting_status_id");
					this.Onitem_accounting_status_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_item_reporting_status_id", DbType="Int NOT NULL")]
		public int item_reporting_status_id
		{
			get
			{
				return this._item_reporting_status_id;
			}
			set
			{
				if ((this._item_reporting_status_id != value))
				{
					this.Onitem_reporting_status_idChanging(value);
					this.SendPropertyChanging();
					this._item_reporting_status_id = value;
					this.SendPropertyChanged("item_reporting_status_id");
					this.Onitem_reporting_status_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_total_revenue", AutoSync=AutoSync.Always, DbType="Decimal(32,10)", IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<decimal> total_revenue
		{
			get
			{
				return this._total_revenue;
			}
			set
			{
				if ((this._total_revenue != value))
				{
					this.Ontotal_revenueChanging(value);
					this.SendPropertyChanging();
					this._total_revenue = value;
					this.SendPropertyChanged("total_revenue");
					this.Ontotal_revenueChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_total_cost", AutoSync=AutoSync.Always, DbType="Decimal(32,10)", IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<decimal> total_cost
		{
			get
			{
				return this._total_cost;
			}
			set
			{
				if ((this._total_cost != value))
				{
					this.Ontotal_costChanging(value);
					this.SendPropertyChanging();
					this._total_cost = value;
					this.SendPropertyChanged("total_cost");
					this.Ontotal_costChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_margin", AutoSync=AutoSync.Always, DbType="Money", IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		public System.Nullable<decimal> margin
		{
			get
			{
				return this._margin;
			}
			set
			{
				if ((this._margin != value))
				{
					this.OnmarginChanging(value);
					this.SendPropertyChanging();
					this._margin = value;
					this.SendPropertyChanged("margin");
					this.OnmarginChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Affiliate_Item", Storage="_Affiliate", ThisKey="affid", OtherKey="affid", IsForeignKey=true)]
		public Affiliate Affiliate
		{
			get
			{
				return this._Affiliate.Entity;
			}
			set
			{
				Affiliate previousValue = this._Affiliate.Entity;
				if (((previousValue != value) 
							|| (this._Affiliate.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Affiliate.Entity = null;
						previousValue.Items.Remove(this);
					}
					this._Affiliate.Entity = value;
					if ((value != null))
					{
						value.Items.Add(this);
						this._affid = value.affid;
					}
					else
					{
						this._affid = default(int);
					}
					this.SendPropertyChanged("Affiliate");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Affiliate")]
	public partial class Affiliate : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _name;
		
		private int _media_buyer_id;
		
		private int _affid;
		
		private int _currency_id;
		
		private string _email;
		
		private string _add_code;
		
		private string _name2;
		
		private System.Nullable<int> _net_term_type_id;
		
		private int _payment_method_id;
		
		private EntitySet<Item> _Items;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void Onmedia_buyer_idChanging(int value);
    partial void Onmedia_buyer_idChanged();
    partial void OnaffidChanging(int value);
    partial void OnaffidChanged();
    partial void Oncurrency_idChanging(int value);
    partial void Oncurrency_idChanged();
    partial void OnemailChanging(string value);
    partial void OnemailChanged();
    partial void Onadd_codeChanging(string value);
    partial void Onadd_codeChanged();
    partial void Onname2Changing(string value);
    partial void Onname2Changed();
    partial void Onnet_term_type_idChanging(System.Nullable<int> value);
    partial void Onnet_term_type_idChanged();
    partial void Onpayment_method_idChanging(int value);
    partial void Onpayment_method_idChanged();
    #endregion
		
		public Affiliate()
		{
			this._Items = new EntitySet<Item>(new Action<Item>(this.attach_Items), new Action<Item>(this.detach_Items));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="VarChar(255) NOT NULL", CanBeNull=false)]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_media_buyer_id", DbType="Int NOT NULL")]
		public int media_buyer_id
		{
			get
			{
				return this._media_buyer_id;
			}
			set
			{
				if ((this._media_buyer_id != value))
				{
					this.Onmedia_buyer_idChanging(value);
					this.SendPropertyChanging();
					this._media_buyer_id = value;
					this.SendPropertyChanged("media_buyer_id");
					this.Onmedia_buyer_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_affid", DbType="Int NOT NULL")]
		public int affid
		{
			get
			{
				return this._affid;
			}
			set
			{
				if ((this._affid != value))
				{
					this.OnaffidChanging(value);
					this.SendPropertyChanging();
					this._affid = value;
					this.SendPropertyChanged("affid");
					this.OnaffidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_currency_id", DbType="Int NOT NULL")]
		public int currency_id
		{
			get
			{
				return this._currency_id;
			}
			set
			{
				if ((this._currency_id != value))
				{
					this.Oncurrency_idChanging(value);
					this.SendPropertyChanging();
					this._currency_id = value;
					this.SendPropertyChanged("currency_id");
					this.Oncurrency_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_email", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string email
		{
			get
			{
				return this._email;
			}
			set
			{
				if ((this._email != value))
				{
					this.OnemailChanging(value);
					this.SendPropertyChanging();
					this._email = value;
					this.SendPropertyChanged("email");
					this.OnemailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_add_code", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string add_code
		{
			get
			{
				return this._add_code;
			}
			set
			{
				if ((this._add_code != value))
				{
					this.Onadd_codeChanging(value);
					this.SendPropertyChanging();
					this._add_code = value;
					this.SendPropertyChanged("add_code");
					this.Onadd_codeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name2", AutoSync=AutoSync.Always, DbType="VarChar(358) NOT NULL", CanBeNull=false, IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		public string name2
		{
			get
			{
				return this._name2;
			}
			set
			{
				if ((this._name2 != value))
				{
					this.Onname2Changing(value);
					this.SendPropertyChanging();
					this._name2 = value;
					this.SendPropertyChanged("name2");
					this.Onname2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_net_term_type_id", DbType="Int")]
		public System.Nullable<int> net_term_type_id
		{
			get
			{
				return this._net_term_type_id;
			}
			set
			{
				if ((this._net_term_type_id != value))
				{
					this.Onnet_term_type_idChanging(value);
					this.SendPropertyChanging();
					this._net_term_type_id = value;
					this.SendPropertyChanged("net_term_type_id");
					this.Onnet_term_type_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_payment_method_id", DbType="Int NOT NULL")]
		public int payment_method_id
		{
			get
			{
				return this._payment_method_id;
			}
			set
			{
				if ((this._payment_method_id != value))
				{
					this.Onpayment_method_idChanging(value);
					this.SendPropertyChanging();
					this._payment_method_id = value;
					this.SendPropertyChanged("payment_method_id");
					this.Onpayment_method_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Affiliate_Item", Storage="_Items", ThisKey="affid", OtherKey="affid")]
		public EntitySet<Item> Items
		{
			get
			{
				return this._Items;
			}
			set
			{
				this._Items.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Items(Item entity)
		{
			this.SendPropertyChanging();
			entity.Affiliate = this;
		}
		
		private void detach_Items(Item entity)
		{
			this.SendPropertyChanging();
			entity.Affiliate = null;
		}
	}
}
#pragma warning restore 1591