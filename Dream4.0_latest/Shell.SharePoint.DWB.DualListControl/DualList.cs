#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//Filename: DualList.cs
#endregion

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;

namespace Shell.SharePoint.DWB.DualListControl
{

    /// <summary>
    /// Provides the UI for the common requirement of two listboxes with items that move between them.
    /// </summary>    
    [
    DefaultEvent("ItemsMoved"),
    PersistChildren(false),
    ParseChildren(true),
    Designer(typeof(DualListDesigner)),
    SupportsPreviewControl(true),
    Themeable(true),
    ]
    public class DualList : CompositeControl
    {

        #region Events

        /// <summary>
        /// The event that fires when items have been moved.
        /// </summary>
        /// <remarks>
        /// This event fires on the server side when the page is posted back, which is not neccessarily when each individual
        /// </remarks>
        [
        Category("Action"),
        ]
        public event EventHandler ItemsMoved
        {
            add
            {
                Events.AddHandler(EventItemsMoved, value);
            }
            remove
            {
                Events.RemoveHandler(EventItemsMoved, value);
            }
        }

        /// <summary>
        /// Raises the <see cref="ItemsMoved"/> event.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#")]
        protected virtual void OnItemsMoved(EventArgs e)
        {
            EventHandler handler = Events[EventItemsMoved] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// The object which is the key into the Events dictionary for the <see cref="ItemsMoved"/> event.
        /// </summary>
        protected static readonly Object EventItemsMoved = new Object();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets if the control will postback every time an item is moved from one side to the other.
        /// </summary>
        /// <remarks>
        /// <para>The property only has an effect when the browser supports clientscript,
        /// as the script is what stores the changes clientside until the page is posted back by other means.
        /// Setting this property to <c>true</c> will cause uplevel browsers to behave the same as downlevel browsers
        /// always do, in that every button press will cause the browser to post back to move the item from side to side.</para>
        /// <para>This property must be set before or during the PreRender phase in order to have an effect.</para>
        /// </remarks>
        [
        Bindable(true),
        Category("Behavior"),
        DefaultValue(_AutoPostBackDefault),
        Description("Gets or sets if the control will postback every time an item is moved from one side to the other."),
        ]
        public virtual Boolean AutoPostBack
        {
            get
            {
                Object state = ViewState["AutoPostBack"];
                if (state != null)
                {
                    return (Boolean)state;
                }
                return _AutoPostBackDefault;
            }
            set
            {
                ViewState["AutoPostBack"] = value;
            }
        }
        private const Boolean _AutoPostBackDefault = false;

        /// <summary>
        /// Gets or sets the number of rows visible in the lists of the control.
        /// </summary>
        [
        Category("Appearance"),
        DefaultValue(_ListRowsDefault),
        Description("Gets or sets the number of rows visible in the lists of the control."),
        ]
        public virtual int ListRows
        {
            get
            {
                Object state = ViewState["ListRows"];
                if (state != null)
                {
                    return (int)state;
                }
                return _ListRowsDefault;
            }
            set
            {
                ViewState["ListRows"] = value;
            }
        }
        private const int _ListRowsDefault = 8;

        /// <summary>
        /// Gets or sets the visibility of the buttons for moving all items between the lists.
        /// </summary>
        [
        Description("Gets or sets the visibility of the buttons for moving all items between the lists."),
        Category("Behavior"),
        DefaultValue(false),
        ]
        public virtual Boolean EnableMoveAll
        {
            get
            {
                Object savedState = this.ViewState["EnableMoveAll"];
                if (savedState != null)
                {
                    return (Boolean)savedState;
                }
                return false;
            }
            set
            {
                this.ViewState["EnableMoveAll"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the buttons for moving items up and down within the list on the right side of the control.
        /// </summary>
        [
        Description("Gets or sets the visibility of the buttons for moving items up and down within the RightBox."),
        Category("Behavior"),
        DefaultValue(false),
        ]
        public virtual Boolean EnableMoveUpDown
        {
            get
            {
                Object savedState = this.ViewState["EnableMoveUpDown"];
                if (savedState != null)
                {
                    return (Boolean)savedState;
                }
                return false;
            }
            set
            {
                this.ViewState["EnableMoveUpDown"] = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="WebControl.ControlStyle"/> of the buttons contained in the control.
        /// </summary>
        [
        Category("Style"),
        Description("Gets the WebControl.ControlStyle of the buttons contained in the control."),
        DefaultValue(null),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        NotifyParentProperty(true),
        ]
        public Style ButtonStyle
        {
            get
            {
                if (_buttonStyle == null)
                {
                    _buttonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_buttonStyle).TrackViewState();
                    }
                }
                return _buttonStyle;
            }
        }
        private Style _buttonStyle;

        /// <summary>
        /// Overrides <see cref="WebControl.TagKey"/>
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Table;
            }
        }

        /// <summary>
        /// Get or Sets whether the items in Right Box should be execluded in Left Box.
        /// </summary>
        [
        Description("Get or Sets whether the items in Right Box should be execluded in Left Box"),
        Category("Behavior"),
        DefaultValue(false),
        ]
        public virtual Boolean ExcludeItemInRightBox
        {
            get
            {
                Object execludeItems = this.ViewState["ExcludeItemInRightBox"];
                if (execludeItems != null)
                {
                    return (Boolean)execludeItems;
                }
                return false;
            }
            set
            {
                this.ViewState["ExcludeItemInRightBox"] = value;
            }
        }

        /// <summary>
        /// Get or Sets whether the items in Right Box should be execluded in Left Box based on Value or Not.
        /// </summary>
        [
        Description("Get or Sets whether the items in Right Box should be execluded in Left Box based on Value or Not"),
        Category("Behavior"),
        DefaultValue(false),
        ]
        public virtual Boolean ExcludeBasedOnValue
        {
            get
            {
                Object execludeItems = this.ViewState["ExcludeBasedOnValue"];
                if (execludeItems != null)
                {
                    return (Boolean)execludeItems;
                }
                return false;
            }
            set
            {
                this.ViewState["ExcludeBasedOnValue"] = value;
            }
        }

        /// <summary>
        /// Get or Sets whether the items in Right Box should be execluded in Left Box based on Text or Not.
        /// </summary>
        [
        Description("Get or Sets whether the items in Right Box should be execluded in Left Box based on Text or Not"),
        Category("Behavior"),
        DefaultValue(false),
        ]
        public virtual Boolean ExcludeBasedOnText
        {
            get
            {
                Object execludeItems = this.ViewState["ExcludeBasedOnText"];
                if (execludeItems != null)
                {
                    return (Boolean)execludeItems;
                }
                return false;
            }
            set
            {
                this.ViewState["ExcludeBasedOnText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the buttons for moving items up and down within the list on the right side of the control.
        /// </summary>
        [
        Description("Gets or sets the visibility of the Left Box"),
        Category("Behavior"),
        DefaultValue(false),
        ]
        public virtual Boolean ShowLeftBox
        {
            get
            {
                Object savedState = this.ViewState["ShowLeftBox"];
                if (savedState != null)
                {
                    return (Boolean)savedState;
                }
                return false;
            }
            set
            {
                this.ViewState["ShowLeftBox"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the buttons for moving items up and down within the list on the right side of the control.
        /// </summary>
        [
        Description("Gets or sets the visibility of the Left Box"),
        Category("Behavior"),
        DefaultValue(false),
        ]
        public virtual Boolean EnableMoveLeft
        {
            get
            {
                Object savedState = this.ViewState["EnableMoveLeft"];
                if (savedState != null)
                {
                    return (Boolean)savedState;
                }
                return false;
            }
            set
            {
                this.ViewState["EnableMoveLeft"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the buttons for moving items up and down within the list on the right side of the control.
        /// </summary>
        [
        Description("Gets or sets the visibility of the Left Box"),
        Category("Behavior"),
        DefaultValue(false),
        ]
        public virtual Boolean EnableMoveRight
        {
            get
            {
                Object savedState = this.ViewState["EnableMoveRight"];
                if (savedState != null)
                {
                    return (Boolean)savedState;
                }
                return false;
            }
            set
            {
                this.ViewState["EnableMoveRight"] = value;
            }
        }


        #region LeftBox

        /// <summary>
        /// Gets or sets the DataSource of the list on the left side of the control.
        /// </summary>
        [
        Browsable(false),
        Description("Gets or sets the DataSource of the list on the left side of the control."),
        Bindable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Themeable(false),
        DefaultValue(null),
        TypeConverter(typeof(DataSourceConverter)),
        ]
        public virtual Object LeftDataSource
        {
            get
            {
                return this.leftBox.DataSource;
            }
            set
            {
                this.leftBox.DataSource = value;
            }
        }

        /// <summary>
        /// The control ID of an IDataSource that will be used as the data source of the left side of the control.
        /// </summary>
        [
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member"),
        IDReferenceProperty(typeof(DataSourceControl)),
        Description("The control ID of an IDataSource that will be used as the data source of the left side of the control."),
        DefaultValue(""),
        Themeable(false),
        Category("Data"),
        TypeConverter(typeof(DataSourceIDConverter)),
        ]
        public virtual String LeftDataSourceID
        {
            get
            {
                return this.leftBox.DataSourceID;
            }
            set
            {
                this.leftBox.DataSourceID = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataMember of the list on the left side of the control.
        /// </summary>
        [
        Description("Gets or sets the DataMember of the list on the left side of the control."),
        Category("Data"),
        DefaultValue(""),
        ]
        public virtual String LeftDataMember
        {
            get
            {
                return this.leftBox.DataMember;
            }
            set
            {
                this.leftBox.DataMember = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataTextField of the list on the left side of the control.
        /// </summary>
        [
        Description("Gets or sets the DataTextField of the list on the left side of the control."),
        Category("Data"),
        DefaultValue(""),
        ]
        public virtual String LeftDataTextField
        {
            get
            {
                return this.leftBox.DataTextField;
            }
            set
            {
                this.leftBox.DataTextField = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataValueField of the list on the left side of the control.
        /// </summary>
        [
        Description("Gets or sets the DataValueField of the list on the left side of the control."),
        Category("Data"),
        DefaultValue(""),
        ]
        public virtual String LeftDataValueField
        {
            get
            {
                return this.leftBox.DataValueField;
            }
            set
            {
                this.leftBox.DataValueField = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataTextFormatString of the list on the left side of the control.
        /// </summary>
        [
        Description("Gets or sets the DataTextFormatString of the list on the left side of the control."),
        Category("Data"),
        DefaultValue(""),
        ]
        public virtual String LeftDataTextFormatString
        {
            get
            {
                return this.leftBox.DataTextFormatString;
            }
            set
            {
                this.leftBox.DataTextFormatString = value;
            }
        }

        /// <summary>
        /// Gets the items in the list the left side of the control.
        /// </summary>
        [
        Description("Gets the items in the list the left side of the control."),
        DefaultValue(null),
        MergableProperty(false),
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        ]
        public virtual ListItemCollection LeftItems
        {
            get
            {
                return this.leftBox.Items;
            }
        }

        /// <summary>
        /// Gets the <see cref="WebControl.ControlStyle"/> of the list on left side of the control.
        /// </summary>
        [
        Description("Gets the ControlStyle of the list on left side of the control."),
        Category("Style"),
        DefaultValue(null),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        NotifyParentProperty(true),
        ]
        public virtual Style LeftListStyle
        {
            get
            {
                if (_leftListStyle == null)
                {
                    _leftListStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_leftListStyle).TrackViewState();
                    }
                }
                return _leftListStyle;
            }
        }
        private Style _leftListStyle;

        #endregion

        #region RightBox
        /// <summary>
        /// Gets or sets the DataSource of the list on the right side of the control.
        /// </summary>
        [
        Browsable(false),
        Bindable(true),
        Description("Gets or sets the DataSource of the list on the right side of the control."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Themeable(false),
        DefaultValue(null),
        TypeConverter(typeof(DataSourceConverter)),
        ]
        public virtual Object RightDataSource
        {
            get
            {
                return this.rightBox.DataSource;
            }
            set
            {
                this.rightBox.DataSource = value;
            }
        }

        /// <summary>
        /// The control ID of an IDataSource that will be used as the data source of the right side of the control.
        /// </summary>
        [
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member"),
        IDReferenceProperty(typeof(DataSourceControl)),
        Description("The control ID of an IDataSource that will be used as the data source of the right side of the control."),
        DefaultValue(""),
        Themeable(false),
        Category("Data"),
        TypeConverter(typeof(DataSourceIDConverter)),
        ]
        public virtual String RightDataSourceID
        {
            get
            {
                return this.rightBox.DataSourceID;
            }
            set
            {
                this.rightBox.DataSourceID = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataMember of the list on the right side of the control.
        /// </summary>
        [
        Description("Gets or sets the DataMember of the list on the right side of the control."),
        Category("Data"),
        DefaultValue(""),
        ]
        public virtual String RightDataMember
        {
            get
            {
                return this.rightBox.DataMember;
            }
            set
            {
                this.rightBox.DataMember = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataTextField of the list on the right side of the control.
        /// </summary>
        [
        Description("Gets or sets the DataTextField of the list on the right side of the control."),
        Category("Data"),
        DefaultValue(""),
        ]
        public virtual String RightDataTextField
        {
            get
            {
                return this.rightBox.DataTextField;
            }
            set
            {
                this.rightBox.DataTextField = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataValueField of the list on the right side of the control.
        /// </summary>
        [
        Description("Gets or sets the DataValueField of the list on the right side of the control."),
        Category("Data"),
        DefaultValue(""),
        ]
        public virtual String RightDataValueField
        {
            get
            {
                return this.rightBox.DataValueField;
            }
            set
            {
                this.rightBox.DataValueField = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataTextFormatString of the list on the right side of the control.
        /// </summary>
        [
        Description("Gets or sets the DataTextFormatString of the list on the right side of the control."),
        Category("Data"),
        DefaultValue(""),
        ]
        public virtual String RightDataTextFormatString
        {
            get
            {
                return this.rightBox.DataTextFormatString;
            }
            set
            {
                this.rightBox.DataTextFormatString = value;
            }
        }

        /// <summary>
        /// Gets the items in the list the right side of the control.
        /// </summary>
        [
        Description("Gets the items in the list the right side of the control."),
        DefaultValue(null),
        MergableProperty(false),
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        ]
        public virtual ListItemCollection RightItems
        {
            get
            {
                return this.rightBox.Items;
            }
        }

        /// <summary>
        /// Gets the <see cref="WebControl.ControlStyle"/> of the list on right side of the control.
        /// </summary>
        [
        Description("Gets the ControlStyle of the list on right side of the control."),
        Category("Style"),
        DefaultValue(null),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        NotifyParentProperty(true),
        ]
        public virtual Style RightListStyle
        {
            get
            {
                if (_rightListStyle == null)
                {
                    _rightListStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_rightListStyle).TrackViewState();
                    }
                }
                return _rightListStyle;
            }
        }
        private Style _rightListStyle;

        #endregion

        #region Text Properties

        /// <summary>
        /// Gets or sets the text displayed above the left list.
        /// </summary>
        [
        Bindable(true),
        Description("Gets or sets the text displayed above the left list."),
        Category("Appearance"),
        DefaultValue("AvailableItems"),
        ]
        public virtual String LeftListLabelText
        {
            get
            {
                Object savedState = this.ViewState["LeftListLabelText"];
                if (savedState != null)
                {
                    return (String)savedState;
                }
                return "Available Items";
            }
            set
            {
                this.ViewState["LeftListLabelText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed above the right list.
        /// </summary>
        [
        Bindable(true),
        Description("Gets or sets the text displayed above the right list."),
        Category("Appearance"),
        DefaultValue("ChosenItems"),
        ]
        public virtual String RightListLabelText
        {
            get
            {
                Object savedState = this.ViewState["RightListLabelText"];
                if (savedState != null)
                {
                    return (String)savedState;
                }
                return "Chosen Items";
            }
            set
            {
                this.ViewState["RightListLabelText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on the button which moves the selected items to the right listbox.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("AddItem"),
        Description("Gets or sets the text displayed on the button which moves the selected items to the right listbox."),
        ]
        public virtual String MoveRightButtonText
        {
            get
            {
                Object state = ViewState["MoveRightButtonText"];
                if (state != null)
                {
                    return (String)state;
                }
                return "Add ->";
            }
            set
            {
                ViewState["MoveRightButtonText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on the button which moves all items to the right listbox.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("AddAll"),
        Description("Gets or sets the text displayed on the button which moves all items to the right listbox."),
        ]
        public virtual String MoveAllRightButtonText
        {
            get
            {
                Object state = ViewState["MoveAllRightButtonText"];
                if (state != null)
                {
                    return (String)state;
                }
                return "Add All ->>";
            }
            set
            {
                ViewState["MoveAllRightButtonText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on the button which moves the selected items to the left listbox.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("RemoveItem"),
        Description("Gets or sets the text displayed on the button which moves the selected items to the left listbox."),
        ]
        public virtual String MoveLeftButtonText
        {
            get
            {
                Object state = ViewState["MoveLeftButtonText"];
                if (state != null)
                {
                    return (String)state;
                }
                return "<- Remove";
            }
            set
            {
                ViewState["MoveLeftButtonText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on the button which moves all the items to the left listbox.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("RemoveAll"),
        Description("Gets or sets the text displayed on the button which moves all the items to the left listbox."),
        ]
        public virtual String MoveAllLeftButtonText
        {
            get
            {
                Object state = ViewState["MoveAllLeftButtonText"];
                if (state != null)
                {
                    return (String)state;
                }
                return "<<- Remove All";
            }
            set
            {
                ViewState["MoveAllLeftButtonText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on the button which moves the selected item up in its listbox.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("MoveUp"),
        Description("Gets or sets the text displayed on the button which moves the selected item up in its listbox."),
        ]
        public virtual String MoveUpButtonText
        {
            get
            {
                Object state = ViewState["MoveUpButtonText"];
                if (state != null)
                {
                    return (String)state;
                }
                return "Up";
            }
            set
            {
                ViewState["MoveUpButtonText"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on the button which moves the selected item down in its listbox.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("MoveDown"),
        Description("Gets or sets the text displayed on the button which moves the selected item down in its listbox."),
        ]
        public virtual String MoveDownButtonText
        {
            get
            {
                Object state = ViewState["MoveDownButtonText"];
                if (state != null)
                {
                    return (String)state;
                }
                return "Down";
            }
            set
            {
                ViewState["MoveDownButtonText"] = value;
            }
        }

        #endregion

        #endregion

        #region ViewState

        /// <exclude />
        protected override object SaveViewState()
        {
            Object[] state = new Object[] { 
				base.SaveViewState()
				,( this._buttonStyle != null ) ? ( (IStateManager)this._buttonStyle ).SaveViewState() : null 
				,( this._leftListStyle != null ) ? ( (IStateManager)this._leftListStyle ).SaveViewState() : null 
				,( this._rightListStyle != null ) ? ( (IStateManager)this._rightListStyle ).SaveViewState() : null 
			};
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] != null)
                {
                    return state;
                }
            }
            return null;
        }

        /// <exclude />
        protected override void LoadViewState(object savedState)
        {
            Object[] state = savedState as Object[];
            if (state == null)
            {
                base.LoadViewState(savedState);
                return;
            }

            base.LoadViewState(state[0]);
            if (state[1] != null)
            {
                ((IStateManager)this.ButtonStyle).LoadViewState(state[1]);
            }
            if (state[2] != null)
            {
                ((IStateManager)this.LeftListStyle).LoadViewState(state[2]);
            }
            if (state[3] != null)
            {
                ((IStateManager)this.RightListStyle).LoadViewState(state[3]);
            }

        }

        /// <exclude />
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_buttonStyle != null)
            {
                ((IStateManager)_buttonStyle).TrackViewState();
            }
            if (_leftListStyle != null)
            {
                ((IStateManager)_leftListStyle).TrackViewState();
            }
            if (_rightListStyle != null)
            {
                ((IStateManager)_rightListStyle).TrackViewState();
            }

        }

        #endregion

        #region Life Cycle

        /// <summary>
        /// Overrides <see cref="Control.CreateChildControls"/> to implement the Composite Control Pattern
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the contained controls.
        /// </summary>
        private void InitializeComponent()
        {
            // Instantiate
            this._leftBox = new DynamicListBox();
            this._rightBox = new DynamicListBox();
            this._moveRight = new Button();
            this._moveLeft = new Button();
            this._moveAllRight = new Button();
            this._moveAllLeft = new Button();
            this._moveUp = new ImageButton();
            this._moveDown = new ImageButton();
            this._leftBoxLabel = new Label();
            this._rightBoxLabel = new Label();
            this._allLeftContainer = new PlaceHolder();
            this._allRightContainer = new PlaceHolder();
            this._enableMoveLeft = new PlaceHolder();
            this._enableMoveRight = new PlaceHolder();
            this._showLeftBox = new PlaceHolder();

            // Customize
            this._leftBox.ID = "LeftBox";
            this._leftBox.SelectionMode = ListSelectionMode.Multiple;
            this._leftBox.ItemsChanged += new EventHandler(leftBox_ItemsChanged);

            this._rightBox.ID = "RightBox";
            this._rightBox.SelectionMode = ListSelectionMode.Multiple;
            this._rightBox.ItemsChanged += new EventHandler(rightBox_ItemsChanged);

            this._moveRight.ID = "MoveRight";
            this._moveRight.CssClass = "DWBDualListMoveRightButton";
            this._moveRight.CausesValidation = false;
            this._moveRight.Click += new EventHandler(moveRight_Click);

            this._moveAllRight.ID = "MoveAllRight";
            this._moveAllRight.CssClass = "DWBDualListMoveAllRightButton";
            this._moveAllRight.CausesValidation = false;
            this._moveAllRight.Click += new EventHandler(moveAllRight_Click);

            this._moveLeft.ID = "MoveLeft";
            this._moveLeft.CssClass = "DWBDualListMoveLefttButton";
            this._moveLeft.CausesValidation = false;
            this._moveLeft.Click += new EventHandler(moveLeft_Click);

            this._moveAllLeft.ID = "MoveAllLeft";
            this._moveAllLeft.CssClass = "DWBDualListMoveAllLefttButton";
            this._moveAllLeft.CausesValidation = false;
            this._moveAllLeft.Click += new EventHandler(moveAllLeft_Click);

            this._moveUp.ID = "MoveUp";
             
            this._moveUp.CausesValidation = false;
            this._moveUp.ImageUrl = "/_layouts/DREAM/images/ARRUPA.GIF";
            this._moveUp.CssClass = "DWBDualListUpButton";
            this._moveUp.Width = Unit.Parse("25px", System.Globalization.CultureInfo.InvariantCulture);
            this._moveUp.Click += new ImageClickEventHandler(moveUp_Click);

            this._moveDown.ID = "MoveDown";
            
            this._moveDown.ImageUrl = "/_layouts/DREAM/images/ARRDOWN.GIF";
            this._moveDown.CausesValidation = false;
            this._moveDown.CssClass = "DWBDualListDownButton";
            this._moveDown.Width = Unit.Parse("25px", System.Globalization.CultureInfo.InvariantCulture);
           this._moveDown.Click += new ImageClickEventHandler(moveDown_Click);

            this.SetEditableChildProperties();

            // Layout
            TableRow topRow = new TableRow();
            this.Controls.Add(topRow);
            topRow.Cells.AddRange(new TableCell[] { new TableCell(), new TableCell() });
            topRow.Cells[0].ColumnSpan = 2;
            topRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            topRow.Cells[0].Font.Bold = true;
            topRow.Cells[0].Controls.Add(this._leftBoxLabel);
            topRow.Cells[1].ColumnSpan = 2;
            topRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            topRow.Cells[1].Font.Bold = true;
            topRow.Cells[1].Controls.Add(this._rightBoxLabel);

            TableRow mainRow = new TableRow();
            this.Controls.Add(mainRow);
            mainRow.Cells.AddRange(new TableCell[] { new TableCell(), new TableCell(), new TableCell(), new TableCell() });

            TableCell currentCell;

            currentCell = mainRow.Cells[0];

            /// Newly Added on 29th Dec
          //  this._showLeftBox.Controls.Add(new LiteralControl("<br/>"));
          //  this._showLeftBox.Controls.Add(new LiteralControl("<br/>"));
            this._showLeftBox.Controls.Add(leftBox);
            currentCell.Controls.Add(this._showLeftBox);
          //  currentCell.Controls.Add(new LiteralControl("<br>"));
          //  currentCell.Controls.Add(new LiteralControl("<br>"));

           /// currentCell.Controls.Add(leftBox); 
            currentCell.HorizontalAlign = HorizontalAlign.Left;

            currentCell = mainRow.Cells[1];
            /// Newly Added on 29th Dec
         //   this._enableMoveRight.Controls.Add(new LiteralControl("<br/>"));
            this._enableMoveRight.Controls.Add(new LiteralControl("<br/>"));
            this._enableMoveRight.Controls.Add(moveRight);
            currentCell.Controls.Add(this._enableMoveRight);
            currentCell.Controls.Add(new LiteralControl("<br>"));
            currentCell.Controls.Add(new LiteralControl("<br>"));
           /// currentCell.Controls.Add(moveRight);
           
          //  this._allRightContainer.Controls.Add(new LiteralControl("<br>"));
            this._allRightContainer.Controls.Add(new LiteralControl("<br>"));
            this._allRightContainer.Controls.Add(this._moveAllRight);
            currentCell.Controls.Add(this._allRightContainer);
            currentCell.Controls.Add(new LiteralControl("<br>"));
            currentCell.Controls.Add(new LiteralControl("<br>"));

            /// Newly Added on 29th Dec
         //   this._enableMoveLeft.Controls.Add(new LiteralControl("<br/>"));
            this._enableMoveLeft.Controls.Add(new LiteralControl("<br/>"));
            this._enableMoveLeft.Controls.Add(moveLeft);
            currentCell.Controls.Add(this._enableMoveLeft);
            currentCell.Controls.Add(new LiteralControl("<br>"));
            currentCell.Controls.Add(new LiteralControl("<br>"));
            /// currentCell.Controls.Add(moveLeft);
            
         //   this._allLeftContainer.Controls.Add(new LiteralControl("<br>"));
            this._allLeftContainer.Controls.Add(new LiteralControl("<br>"));
            this._allLeftContainer.Controls.Add(this._moveAllLeft);
            currentCell.Controls.Add(this._allLeftContainer);
            currentCell.HorizontalAlign = HorizontalAlign.Center;
            currentCell.VerticalAlign = VerticalAlign.Middle;

            currentCell = mainRow.Cells[2];
            currentCell.Controls.Add(rightBox);
            currentCell.HorizontalAlign = HorizontalAlign.Right;

            currentCell = mainRow.Cells[3];
            currentCell.Controls.Add(this._moveUp);
            currentCell.Controls.Add(new LiteralControl("<br>"));
            currentCell.Controls.Add(this._moveDown);
            currentCell.HorizontalAlign = HorizontalAlign.Left;
            currentCell.VerticalAlign = VerticalAlign.Middle;

        }

        void moveDown_Click(object sender, ImageClickEventArgs e)
        {
            Int32 originalIndex = this.rightBox.SelectedIndex;
            if (originalIndex > 0)
            {
                ListItem movedItem = this.rightBox.SelectedItem;
                this.rightBox.Items.Remove(movedItem);
                this.rightBox.Items.Insert(originalIndex - 1, movedItem);
                this.EnsureChangeEvent();
            }
          //  throw new Exception("The method or operation is not implemented.");
        }

        void moveUp_Click(object sender, ImageClickEventArgs e)
        {
            Int32 originalIndex = this.rightBox.SelectedIndex;
            if (originalIndex > 0)
            {
                ListItem movedItem = this.rightBox.SelectedItem;
                this.rightBox.Items.Remove(movedItem);
                this.rightBox.Items.Insert(originalIndex - 1, movedItem);
                this.EnsureChangeEvent();
            }
            //throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// FixAvailableItems is called directly before <see cref="Render"/> to make sure that none of the items on the right, "chosen", list exist in the left, "available" list.
        /// </summary>
        protected virtual void FixAvailableItems()
        {
            if (this.DesignMode)
            {
                return;
            }
            if (ExcludeItemInRightBox && ExcludeBasedOnText)
            {
                foreach (ListItem item in this.rightBox.Items)
                {
                    ListItem leftItem = this.leftBox.Items.FindByText(item.Text);
                    if (leftItem != null && leftItem.Text == item.Text)
                    {
                        this.leftBox.Items.Remove(leftItem);
                    }
                }
            }
            else
            {
                foreach (ListItem item in this.rightBox.Items)
                {
                    ListItem leftItem = this.leftBox.Items.FindByValue(item.Value);
                    if (leftItem != null && leftItem.Text == item.Text)
                    {
                        this.leftBox.Items.Remove(leftItem);
                    }
                }
            }
        }

        /// <summary>
        /// Overrides <see cref="Control.OnPreRender"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#")]
        protected override void OnPreRender(EventArgs e)
        {
            this.FixAvailableItems();
            base.OnPreRender(e);
            this.RegisterScript();
        }

        /// <summary>
        /// Overrides <see cref="Control.Render"/>.
        /// </summary>
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Page != null)
            {
                this.Page.VerifyRenderingInServerForm(this);
            }

            this.SetEditableChildProperties();
            //this.FixAvailableItems();

            base.Render(writer);
        }

        /// <summary>
        /// Overrides <see cref="WebControl.CreateControlStyle"/>.
        /// </summary>
        protected override Style CreateControlStyle()
        {
            return new TableStyle(this.ViewState);
        }

        /// <summary>
        /// Sets the current property values on child controls directly before rendering.
        /// </summary>
        protected virtual void SetEditableChildProperties()
        {

            this.leftBox.Rows = this.ListRows;
            this.rightBox.Rows = this.ListRows;

            this.leftBoxLabel.Text = this.LeftListLabelText;
            this.rightBoxLabel.Text = this.RightListLabelText;
            this.moveRight.Text = this.MoveRightButtonText;
            this.moveAllRight.Text = this.MoveAllRightButtonText;
            this.moveLeft.Text = this.MoveLeftButtonText;
            this.moveAllLeft.Text = this.MoveAllLeftButtonText;
            //this.moveUp.Text = this.MoveUpButtonText;
            //this.moveDown.Text = this.MoveDownButtonText;

            this.allRightContainer.Visible = this.EnableMoveAll;
            this.allLeftContainer.Visible = this.EnableMoveAll;

            /// To show/hide left/right arrows and left list box
            this.enableMoveLeft.Visible = this.EnableMoveLeft;
            this.enableMoveRight.Visible = this.EnableMoveRight;
            this.showLeftBox.Visible = this.ShowLeftBox;

            if (this.moveUp.Parent != null)
            {
                this.moveUp.Parent.Visible = this.EnableMoveUpDown;
            }

            if (_buttonStyle != null)
            {
                this.moveRight.ControlStyle.CopyFrom(this._buttonStyle);
                this.moveAllRight.ControlStyle.CopyFrom(this._buttonStyle);
                this.moveLeft.ControlStyle.CopyFrom(this._buttonStyle);
                this.moveAllLeft.ControlStyle.CopyFrom(this._buttonStyle);
                this.moveUp.ControlStyle.CopyFrom(this._buttonStyle);
                this.moveDown.ControlStyle.CopyFrom(this._buttonStyle);
            }

            if (_leftListStyle != null)
            {
                this.leftBox.ControlStyle.CopyFrom(this._leftListStyle);
            }

            if (_rightListStyle != null)
            {
                this.rightBox.ControlStyle.CopyFrom(this._rightListStyle);
            }

        }

        #endregion

        #region ClientScript

        /// <summary>
        /// Registers the script for this control.
        /// </summary>
        /// <remarks>
        /// <para>The script will not be emitted if AutoPostBack is set to true, 
        /// as the script's sole purpose is to keep the browser from posting back as the user moves items.
        /// </para>
        /// </remarks>
        protected virtual void RegisterScript()
        {
            if (this.Page != null && !this.AutoPostBack)
            {
                ClientScriptManager script = Page.ClientScript;
                //script.RegisterClientScriptResource(typeof(DualList), "WebControlLibrary1.Javascripts.DualListScript.js");
                script.RegisterStartupScript(typeof(DualList), "DualListScript", "MetaBuilders_DualList_Init(); " + String.Format("if ( typeof(Sys) != 'undefined' ) {{ Sys.WebForms.PageRequestManager.getInstance().add_endRequest({0}); }}", "MetaBuilders_DualList_Init"), true);

                StringBuilder objInit = new StringBuilder();
                objInit.Append("{ LeftBoxID:'");
                objInit.Append(this.leftBox.ClientID);
                objInit.Append("', ");
                objInit.Append("RightBoxID:'");
                objInit.Append(this.rightBox.ClientID);
                objInit.Append("', ");
                objInit.Append("MoveRightID:'");
                objInit.Append(this.moveRight.ClientID);
                objInit.Append("', ");
                objInit.Append("MoveAllRightID:'");
                objInit.Append(this.moveAllRight.ClientID);
                objInit.Append("', ");
                objInit.Append("MoveLeftID:'");
                objInit.Append(this.moveLeft.ClientID);
                objInit.Append("', ");
                objInit.Append("MoveAllLeftID:'");
                objInit.Append(this.moveAllLeft.ClientID);
                objInit.Append("', ");
                objInit.Append("MoveUpID:'");
                objInit.Append(this.moveUp.ClientID);
                objInit.Append("', ");
                objInit.Append("MoveDownID:'");
                objInit.Append(this.moveDown.ClientID);
                objInit.Append("' }");
                script.RegisterArrayDeclaration("MetaBuilders_DualLists", objInit.ToString());
            }
        }

        #endregion

        #region Child Control Event Handlers
        // These handlers will only fire if the client browser doesn't support clientscript

        private void moveRight_Click(object sender, EventArgs e)
        {
            MoveSelectedItems(this.leftBox, this.rightBox);
            this.EnsureChangeEvent();
        }

        private void moveAllRight_Click(object sender, EventArgs e)
        {
            MoveAllItems(this.leftBox, this.rightBox);
            this.EnsureChangeEvent();
        }

        private void moveLeft_Click(object sender, EventArgs e)
        {
            MoveSelectedItems(this.rightBox, this.leftBox);
            this.EnsureChangeEvent();
        }

        private void moveAllLeft_Click(object sender, EventArgs e)
        {
            MoveAllItems(this.rightBox, this.leftBox);
            this.EnsureChangeEvent();
        }

        //private void moveUp_Click(object sender, EventArgs e)
        //{
        //    Int32 originalIndex = this.rightBox.SelectedIndex;
        //    if (originalIndex > 0)
        //    {
        //        ListItem movedItem = this.rightBox.SelectedItem;
        //        this.rightBox.Items.Remove(movedItem);
        //        this.rightBox.Items.Insert(originalIndex - 1, movedItem);
        //        this.EnsureChangeEvent();
        //    }
        //}

        //private void moveDown_Click(object sender, EventArgs e)
        //{
        //    Int32 originalIndex = this.rightBox.SelectedIndex;
        //    if (originalIndex < this.rightBox.Items.Count - 1)
        //    {
        //        ListItem movedItem = this.rightBox.SelectedItem;
        //        this.rightBox.Items.Remove(movedItem);
        //        this.rightBox.Items.Insert(originalIndex + 1, movedItem);
        //        this.EnsureChangeEvent();
        //    }
        //}

        private static void MoveSelectedItems(ListBox source, ListBox target)
        {
            while (source.SelectedIndex != -1)
            {
                ListItem sourceItem = source.SelectedItem;
                source.Items.Remove(sourceItem);

                ListItem targetItem = target.Items.FindByValue(sourceItem.Value);
                if (targetItem == null || targetItem.Text != sourceItem.Text)
                {
                    target.Items.Add(sourceItem);
                }
            }
        }

        private static void MoveAllItems(ListBox source, ListBox target)
        {
            while (source.Items.Count != 0)
            {
                ListItem sourceItem = source.Items[0];
                source.Items.RemoveAt(0);

                ListItem targetItem = target.Items.FindByValue(sourceItem.Value);
                if (targetItem == null || targetItem.Text != sourceItem.Text)
                {
                    target.Items.Add(sourceItem);
                }
            }
        }

        private void leftBox_ItemsChanged(object sender, EventArgs e)
        {
            this.EnsureChangeEvent();
        }

        private void rightBox_ItemsChanged(object sender, EventArgs e)
        {
            this.EnsureChangeEvent();
        }

        private void EnsureChangeEvent()
        {
            if (!this.hasNotifiedOfChange)
            {
                hasNotifiedOfChange = true;
                this.OnItemsMoved(EventArgs.Empty);
            }
        }

        private Boolean hasNotifiedOfChange;
        #endregion

        #region Child Control References
        private DynamicListBox _leftBox;
        private DynamicListBox _rightBox;

        private Button _moveRight;
        private Button _moveLeft;
        private Button _moveAllRight;
        private Button _moveAllLeft;

        private ImageButton _moveUp;
        private ImageButton _moveDown;

        private Label _leftBoxLabel;
        private Label _rightBoxLabel;

        private PlaceHolder _allRightContainer;
        private PlaceHolder _allLeftContainer;

        /// <summary>
        /// Added for hiding left and right move buttons
        /// </summary>
        private PlaceHolder _enableMoveLeft;
        private PlaceHolder _enableMoveRight;
        private PlaceHolder _showLeftBox;

        internal DynamicListBox leftBox
        {
            get
            {
                this.EnsureChildControls();
                return _leftBox;
            }
        }
        internal DynamicListBox rightBox
        {
            get
            {
                this.EnsureChildControls();
                return _rightBox;
            }
        }
        private Button moveRight
        {
            get
            {
                this.EnsureChildControls();
                return _moveRight;
            }
        }
        private Button moveLeft
        {
            get
            {
                this.EnsureChildControls();
                return _moveLeft;
            }
        }
        private Button moveAllRight
        {
            get
            {
                this.EnsureChildControls();
                return _moveAllRight;
            }
        }
        private Button moveAllLeft
        {
            get
            {
                this.EnsureChildControls();
                return _moveAllLeft;
            }
        }

        private ImageButton moveUp
        {
            get
            {
                this.EnsureChildControls();
                return _moveUp;
            }
        }
        private ImageButton moveDown
        {
            get
            {
                this.EnsureChildControls();
                return _moveDown;
            }
        }

        private Label leftBoxLabel
        {
            get
            {
                this.EnsureChildControls();
                return _leftBoxLabel;
            }
        }
        private Label rightBoxLabel
        {
            get
            {
                this.EnsureChildControls();
                return _rightBoxLabel;
            }
        }

        private PlaceHolder allRightContainer
        {
            get
            {
                this.EnsureChildControls();
                return _allRightContainer;
            }
        }
        private PlaceHolder allLeftContainer
        {
            get
            {
                this.EnsureChildControls();
                return _allLeftContainer;
            }
        }

        private PlaceHolder enableMoveLeft
        {
            get
            {
                this.EnsureChildControls();
                return _enableMoveLeft;
            }
        }

        private PlaceHolder enableMoveRight
        {
            get
            {
                this.EnsureChildControls();
                return _enableMoveRight;
            }
        }

        private PlaceHolder showLeftBox
        {
            get
            {
                this.EnsureChildControls();
                return _showLeftBox;
            }
        }
        #endregion
    }
}
