﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SitePlugin;
namespace NicoSitePlugin.Test
{
    public class NicoCommentViewModel2 : CommentViewModelBase, INicoCommentViewModel
    {
        //本当はIUserはコンストラクタから入れたい。でもIUserStoreにはサイト毎の実装はいらない（多分）だろうから共通の場所でやってる。
        //NameItemsはそのユーザの全てのコメントに共通のはずだからcvm内ではなくIUserとかに持たせたい

        private IUser _user;
        public override IUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                if (_user != null)
                {
                    _user.PropertyChanged += (s, e) =>
                    {
                        switch (e.PropertyName)
                        {
                            case nameof(_user.Nickname):
                                SetName();
                                break;
                        }
                    };
                }
            }
        }
        public override string UserId => _chat.user_id;
        private readonly NicoSiteOptions _siteOptions;
        private readonly chat _chat;
        internal NicoCommentViewModel2(ConnectionName connectionName, chat chat, IOptions options, NicoSiteOptions siteOptions) 
            : base(connectionName, options)
        {
            _siteOptions = siteOptions;
            _chat = chat;

            SetName();
            MessageItems = new List<IMessagePart> { new MessageText { Text = _chat.text } };
        }

        private void SetName()
        {
            if (_user == null || string.IsNullOrEmpty(_user.Nickname))
            {
                NameItems = new List<IMessagePart> { new MessageText { Text = _chat.user_id } };
            }
            else
            {
                NameItems = new List<IMessagePart> { new MessageText { Text = _user.Nickname } };
            }
            RaisePropertyChanged(nameof(NameItems));
        }
    }
    public class NicoCommentViewModel : INicoCommentViewModel
    {
        public string ConnectionName => _connectionName.Name;

        public IEnumerable<IMessagePart> NameItems { get; set; }

        public IEnumerable<IMessagePart> MessageItems { get; set; }

        public string Info { get; set; }

        public string Id { get; set; }

        public string UserId
        {
            get
            {
                return _chat.user_id;
            }
        }

        //本当はIUserはコンストラクタから入れたい。でもIUserStoreにはサイト毎の実装はいらない（多分）だろうから共通の場所でやってる。
        //NameItemsはそのユーザの全てのコメントに共通のはずだからcvm内ではなくIUserとかに持たせたい

        private IUser _user;
        public IUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                if (_user != null)
                {
                    _user.PropertyChanged += (s, e) =>
                    {
                        switch (e.PropertyName)
                        {
                            case nameof(_user.Nickname):                                
                                SetName();
                                break;
                        }
                    };
                }
            }
        }

        public bool IsInfo { get; set; }

        public bool IsFirstComment { get; set; }

        public IEnumerable<IMessagePart> Thumbnail => new List<IMessagePart>();

        public FontFamily FontFamily => _options.FontFamily;

        public FontStyle FontStyle => _options.FontStyle;

        public FontWeight FontWeight => _options.FontWeight;

        public int FontSize => _options.FontSize;

        public bool IsVisible { get; set; } = true;

        public SolidColorBrush Foreground => new SolidColorBrush(_options.ForeColor);

        public SolidColorBrush Background => new SolidColorBrush(_options.BackColor);




        public Task AfterCommentAdded()
        {
            throw new NotImplementedException();
        }

        private readonly ConnectionName _connectionName;
        private readonly chat _chat;
        private readonly IOptions _options;
        private readonly NicoSiteOptions _siteOptions;
        internal NicoCommentViewModel(ConnectionName connectionName, chat chat, IOptions options, NicoSiteOptions siteOptions)
        {
            _connectionName = connectionName;
            _chat = chat;
            _options = options;
            _siteOptions = siteOptions;
            SetName();
            MessageItems = new List<IMessagePart> { new MessageText { Text = chat.text } };

        }
        private void SetName()
        {
            if (_user == null || string.IsNullOrEmpty(_user.Nickname))
            {
                NameItems = new List<IMessagePart> { new MessageText { Text = _chat.user_id } };
            }
            else
            {
                NameItems = new List<IMessagePart> { new MessageText { Text = _user.Nickname } };
            }
            RaisePropertyChanged(nameof(NameItems));
        }
        #region INotifyPropertyChanged
        [NonSerialized]
        private System.ComponentModel.PropertyChangedEventHandler _propertyChanged;
        /// <summary>
        /// 
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            _propertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
    class MessageText : IMessageText
    {
        public string Text { get; set; }
    }
}