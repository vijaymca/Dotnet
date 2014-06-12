﻿function MetaBuilders_DualList_Init() {
	if( !(document.getElementById) ) { return; }
	for( var i = 0; i < MetaBuilders_DualLists.length; i++ ) {
		var info = MetaBuilders_DualLists[i];
		MetaBuilders_DualList_Load(info);
	}
}
function MetaBuilders_DualList_Load(info) {
	var leftBox = document.getElementById(info.LeftBoxID);
	var rightBox = document.getElementById(info.RightBoxID);
	var moveRight = document.getElementById(info.MoveRightID);
	var moveAllRight = document.getElementById(info.MoveAllRightID);
	var moveLeft = document.getElementById(info.MoveLeftID);
	var moveAllLeft = document.getElementById(info.MoveAllLeftID);
	var moveUp = document.getElementById(info.MoveUpID);
	var moveDown = document.getElementById(info.MoveDownID);
//	if ( leftBox == null || typeof( leftBox.Parent ) != 'undefined' ) {
//		return;
//	}
	var dualListParent = new Object();
	dualListParent.LeftBox = leftBox;
	dualListParent.RightBox = rightBox;
	dualListParent.MoveRight = moveRight;
	dualListParent.MoveAllRight = moveAllRight;
	dualListParent.MoveLeft = moveLeft;
	dualListParent.MoveAllLeft = moveAllLeft;
	dualListParent.MoveUp = moveUp;
	dualListParent.MoveDown = moveDown;
	dualListParent.SetEnabled = MetaBuilders_DualList_SetEnabled;
	if(leftBox != null)
{
	leftBox.Parent = dualListParent;
	leftBox.MoveSelection = MetaBuilders_DualList_MoveRight;
	leftBox.ondblclick = leftBox.MoveSelection;
}

if(rightBox != null)
{
	rightBox.Parent = dualListParent;
	rightBox.MoveSelection = MetaBuilders_DualList_MoveLeft;
	rightBox.ondblclick = rightBox.MoveSelection;
}
	if ( moveUp != null && moveDown != null ) {
		rightBox.SetUpDownEnabled = MetaBuilders_DualList_SetUpDownEnabled;
		rightBox.onchange = rightBox.SetUpDownEnabled;
	} else {
		rightBox.SetUpDownEnabled = function() {};
	}
	if(moveRight != null)
	{
	moveRight.Parent = dualListParent;
	moveRight.DoCommand = MetaBuilders_DualList_MoveRight;
	moveRight.onclick = moveRight.DoCommand;
	}
	if ( moveAllRight != null ) {
		moveAllRight.Parent = dualListParent;
		moveAllRight.DoCommand = MetaBuilders_DualList_MoveAllRight;
		moveAllRight.onclick = moveAllRight.DoCommand;
	}
	if(moveLeft != null)
	{
	moveLeft.Parent = dualListParent;
	moveLeft.DoCommand = MetaBuilders_DualList_MoveLeft;
	moveLeft.onclick = moveLeft.DoCommand;
	}
	if ( moveAllLeft != null ) {
		moveAllLeft.Parent = dualListParent;
		moveAllLeft.DoCommand = MetaBuilders_DualList_MoveAllLeft;
		moveAllLeft.onclick = moveAllLeft.DoCommand;
	}
	if ( moveUp != null ) {
		moveUp.Parent = dualListParent;
		moveUp.DoCommand = MetaBuilders_DualList_MoveUp;
		moveUp.onclick = moveUp.DoCommand;
	}
	if ( moveDown != null ) {
		moveDown.Parent = dualListParent;
		moveDown.DoCommand = MetaBuilders_DualList_MoveDown;
		moveDown.onclick = moveDown.DoCommand;
	}
	if ( moveRight != null && !moveRight.disabled ) {
		dualListParent.SetEnabled();
		if ( moveUp != null ) {
			rightBox.SetUpDownEnabled();
		}
	}
}
function MetaBuilders_DualList_SetEnabled() {
	var leftItemsEmpty = ( this.LeftBox.options.length == 0 );
	var rightItemsEmpty = ( this.RightBox.options.length == 0 );
	this.MoveRight.disabled = leftItemsEmpty;
	if ( this.MoveAllRight != null ) {
		this.MoveAllRight.disabled = leftItemsEmpty;
	}
	this.MoveLeft.disabled = rightItemsEmpty;
	if ( this.MoveAllLeft != null ) {
		this.MoveAllLeft.disabled = rightItemsEmpty;
	}
	this.RightBox.SetUpDownEnabled();
}
function MetaBuilders_DualList_SetUpDownEnabled() {
	var selectedIndex = this.options.selectedIndex;
	this.Parent.MoveUp.disabled = ( selectedIndex <= 0 );
	this.Parent.MoveDown.disabled = ( selectedIndex == -1 || selectedIndex == this.options.length - 1 );
}
function MetaBuilders_DualList_MoveRight() {
	MetaBuilders_DualList_MoveSelectedItems(this.Parent.LeftBox,this.Parent.RightBox);
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveAllRight() {
	MetaBuilders_DualList_MoveAllItems(this.Parent.LeftBox,this.Parent.RightBox);
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveLeft() {
	MetaBuilders_DualList_MoveSelectedItems(this.Parent.RightBox,this.Parent.LeftBox);
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveAllLeft() {
	MetaBuilders_DualList_MoveAllItemsLeftCustom(this.Parent.RightBox,this.Parent.LeftBox);
	this.Parent.SetEnabled();
	return false;
}
function MetaBuilders_DualList_MoveUp() {
this.Parent.RightBox.MoveUp();
//	this.Parent.SetEnabled();
this.Parent.RightBox.SetUpDownEnabled();
	return false;
}
function MetaBuilders_DualList_MoveDown() {
	this.Parent.RightBox.MoveDown();
	//this.Parent.SetEnabled();
	this.Parent.RightBox.SetUpDownEnabled();
	return false;
}
function MetaBuilders_DualList_MoveSelectedItems(source,target) {
	if ( source.options.length == 0 ) {
		return;
	}
	var originalIndex = source.options.selectedIndex;
	while ( source.options.selectedIndex >= 0 ) {
		MetaBuilders_DualList_MoveItem( source.options.selectedIndex, source, target );
	}
	if ( originalIndex < source.options.length ) {
		source.options.selectedIndex = originalIndex;
	} else {
		source.options.selectedIndex = source.options.length - 1;
	}
	target.options.selectedIndex = target.options.length - 1;
}

function MetaBuilders_DualList_MoveAllItemsLeftCustom(source,target) {

var hidTeamOwnerDetail = document.getElementById(GetObjectID('hidTeamOwnerUserId','input'));
	    if(hidTeamOwnerDetail != null)
	    {
	    i= source.options.length;
	      while ( i >= 1) {
	      /// Multi Team Owner Implementation
            /// Changed By: Yasotha
            /// Date : 20-Jan-2010
	      // if(source.options[i-1].value!= hidTeamOwnerDetail.value)
            if(hidTeamOwnerDetail.value.indexOf(source.options[i-1].value) == -1 )
	          {	         
	          /// Multi Team Owner Implementation
            /// End 
		    MetaBuilders_DualList_MoveItem( i-1, source, target );
		     i = source.options.length;
		      }
		      else
		      {
		     i = i-1;
		      }
	     } 
	 }
	 alert('User is the Team Owner and cannot be removed from the Team.');
}

function MetaBuilders_DualList_MoveAllItems(source,target) {

	while ( source.options.length > 0 ) {
		MetaBuilders_DualList_MoveItem( 0, source, target );
	}
}

function MetaBuilders_DualList_MoveAllItem(itemIndex,source,target) {
	var itemValue = source.options[itemIndex].value;
	var itemText = source.options[itemIndex].text;
	source.Remove( itemIndex );
	target.Add(itemValue, itemText);
}

function MetaBuilders_DualList_MoveItem(itemIndex,source,target) {
	var itemValue = source.options[itemIndex].value;

	var hidTeamOwnerDetail = document.getElementById(GetObjectID('hidTeamOwnerUserId','input'));
	if(hidTeamOwnerDetail != null)
	{
	        /// Multi Team Owner Implementation
            /// Changed By: Yasotha
            /// Date : 20-Jan-2010
//	    if(itemValue == hidTeamOwnerDetail.value)
//	    {
        if(hidTeamOwnerDetail.value.indexOf(itemValue) != -1)
	    {
	    /// Multi Team Owner Implementation
        /// End
	    alert('Selected user is the Team Owner and cannot be removed from the Team.');
	    source.selectedIndex =-1;
	    return false;
	    }
	}
	
	var itemText = source.options[itemIndex].text;
	source.Remove( itemIndex );
	target.Add(itemValue, itemText);
	
}

