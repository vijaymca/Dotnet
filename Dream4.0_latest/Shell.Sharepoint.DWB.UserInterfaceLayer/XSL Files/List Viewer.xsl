<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt">
  <xsl:output method="xml"/>
  <!--DWB Required Parameters-->
  <xsl:param name="listType" select="MasterPage"/>
  <xsl:param name="historyColumn" select="false"/>
  <xsl:param name="editColumn" select="false"/>
  <xsl:param name="listItemAction" select="false"/>
  <xsl:param name="addMasterLinkColumn" select="false"/>
  <xsl:param name="viewMasterLinkColumn" select="false"/>
  <xsl:param name="addTerminateColumn" select="false"/>
  <xsl:param name="activeStatus" select="true"/>
  <xsl:variable name="idValue" select="''"/>
  <xsl:variable name="rowID" select="''"/>
  <!-- Set the number of records per page-->
  <xsl:param name="recordsPerPage" select="0" />
  <!-- Page Number field -->
  <xsl:param name="pageNumber" select="1" />
  <!--max pages-->
  <xsl:param name="MaxPages" select="5"/>
  <!--CurrentPage-->
  <xsl:param name="CurrentPage" select="0"/>
  <!--CurrentPage-->
  <xsl:param name="CurrentPageName" select="''"/>
  <!--No of Records-->
  <xsl:param name="recordCount" select="3"/>
  <xsl:param name="sortBy" select="'Name'"/>
  <xsl:param name="sortType" select="'descending'"/>
  <xsl:param name ="sortDataType" select="'text'"/>
  <xsl:param name ="AuditListName" select="''" />

  <xsl:template match="/">
    <br></br>
    <xsl:variable name="Pages" select="ceiling($recordCount div $recordsPerPage)" />
    <!--To do paging with page numbers-->
    <xsl:variable name="StartPagenumber" select="0"/>
    <!-- Call Templates of for page number Loop-->
    <xsl:if test="$Pages > 1">
      <xsl:choose>
        <xsl:when test="$listType = 'WellBookPageView'">
          <xsl:call-template name="PagingForWellBookPageView">
            <xsl:with-param name="EndPageNumber" select="$Pages"/>
            <xsl:with-param name="StartPagenumber" select="0"/>
          </xsl:call-template>
          <br></br>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="Paging">
            <xsl:with-param name="EndPageNumber" select="$Pages"/>
            <xsl:with-param name="StartPagenumber" select="0"/>
          </xsl:call-template>
          <br></br>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:for-each select="records">
      <div id="tableContainer" class="tableContainer">
        <table style="border-right:1px solid #336699;"  cellpadding="0" cellspacing="0" 
          class="scrollTable" id="tblSearchResults">
          <xsl:for-each select="record[@order=1]">
            <thead class="fixedHeader" id="fixedHeader">
              <!--<tr>
                <th>
                  <xsl:value-of select="$sortBy"/>&#160;                  
                  <xsl:value-of select="$sortType"/>
                </th>
              </tr>-->
              <tr style="height: 20px;">

                <xsl:if test="$addTerminateColumn = 'true'">
                  <th>&#160;</th>
                </xsl:if>
                <xsl:if test="$listType = 'WellBookPageView'">
                  <th style="text-align:left;" class="printerFriendly">
                    <xsl:element name="input">
                      <xsl:attribute name="type">checkbox</xsl:attribute>
                      <xsl:attribute name="name">chbHeader</xsl:attribute>
                      <xsl:attribute name="id">chbHeader</xsl:attribute>
                      <xsl:attribute name="onclick">javascript:SelectUnSelectAllPages(true);</xsl:attribute>
                    </xsl:element>
                  </th>
                </xsl:if>

                <xsl:for-each select="recordInfo/attribute[@display != 'false']">
                  <th>
                    <!--Setting javascript method to column header when list type is equal to WellBookPageView -->
                    <xsl:if test="$listType = 'WellBookPageView'">
                      <xsl:choose>
                        <xsl:when test="$sortBy = @name" >
                          <xsl:choose>
                            <xsl:when test="$sortType = 'ascending'">
                              <xsl:text disable-output-escaping="yes">
        &lt;a href="Javascript:doSortAndPaging('</xsl:text>
                              <xsl:value-of select="$CurrentPageName"/>',
                              <xsl:value-of select="$pageNumber"/>,
                              <xsl:value-of select="$recordCount"/>,
                              '<xsl:value-of select="@name"/>',
                              '<xsl:value-of select="$activeStatus"/>','descending')"
                              <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                              <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes">
        &lt;a href="Javascript:doSortAndPaging('</xsl:text>
                              <xsl:value-of select="$CurrentPageName"/>',
                              <xsl:value-of select="$pageNumber"/>,
                              <xsl:value-of select="$recordCount"/>,
                              '<xsl:value-of select="@name"/>','<xsl:value-of select="$activeStatus"/>','ascending')"
                              <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                              <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes">
                     &lt;a href="Javascript:doSortAndPaging('</xsl:text>
                          <xsl:value-of select="$CurrentPageName"/>',
                          <xsl:value-of select="$pageNumber"/>,
                          <xsl:value-of select="$recordCount"/>,
                          '<xsl:value-of select="@name"/>','<xsl:value-of select="$activeStatus"/>',
                          '<xsl:value-of select="$sortType"/>')"
                          <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:if>
                    <!--Setting javascript method to column header when list type is not equal to WellBookPageView-->
                    <xsl:if test="$listType != 'WellBookPageView'">
                      <xsl:choose>
                        <xsl:when test="$sortBy = @name ">
                          <xsl:choose>
                            <xsl:when test="$sortType = 'ascending'">
                              <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                              <xsl:value-of select="$CurrentPageName"/>',
                              <xsl:value-of select="$pageNumber"/>,
                              <xsl:value-of select="$recordCount"/>,
                              '<xsl:value-of select="@name"/>',
                              '<xsl:value-of select="$activeStatus"/>',
                              'descending')"
                              <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                              <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                              <xsl:value-of select="$CurrentPageName"/>',
                              <xsl:value-of select="$pageNumber"/>,
                              <xsl:value-of select="$recordCount"/>,
                              '<xsl:value-of select="@name"/>','<xsl:value-of select="$activeStatus"/>',
                              'ascending')"
                              <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                              <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                          <xsl:value-of select="$CurrentPageName"/>',
                          <xsl:value-of select="$pageNumber"/>,
                          <xsl:value-of select="$recordCount"/>,
                          '<xsl:value-of select="@name"/>','<xsl:value-of select="$activeStatus"/>',
                          '<xsl:value-of select="$sortType"/>')"
                          <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:if>
                  </th>
                </xsl:for-each>
                <xsl:call-template name ="CreateFuctionHeaders" />

                <xsl:if test="$addMasterLinkColumn = 'true'">
                  <th>Master Pages</th>
                </xsl:if>
                <xsl:if test="$viewMasterLinkColumn = 'true'">
                  <th>Master Pages</th>
                </xsl:if>
              </tr>
            </thead>
          </xsl:for-each>
          <tbody boder ="1" class="scrollContent">
            <xsl:for-each select="record">
              <xsl:sort select="recordInfo/attribute[@name = $sortBy]/@value" order="{$sortType}" data-type="{$sortDataType}"/>
              <xsl:if test="position() &gt; $recordsPerPage * number($pageNumber) and position()
                    &lt;= number($recordsPerPage * number($pageNumber) + $recordsPerPage)">
                <xsl:variable name="rowID">
                  <xsl:value-of select="number(number(@order) - $recordsPerPage * number($pageNumber))"/>
                </xsl:variable>
                <xsl:variable name="idValue">
                  <xsl:value-of select="@recordNumber"/>
                </xsl:variable>
                <tr height="20px">
                  <xsl:attribute name="ID">
                    <xsl:value-of select="concat('headertr' ,$idValue)"/>
                  </xsl:attribute>
                  <xsl:choose>
                    <xsl:when test="position() mod 2 = 1">
                      <xsl:attribute name="class">evenRowStyle</xsl:attribute>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:attribute name="class">oddRowStyle</xsl:attribute>
                    </xsl:otherwise>
                  </xsl:choose>
                  <xsl:if test="$listType = 'WellBookPageView'">
                    <xsl:call-template name="CreateCheckBoxColumn">
                      <xsl:with-param name="ID" select="$idValue"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:if>

                  <xsl:for-each select="recordInfo/attribute[@display != 'false']">
                    <xsl:choose>
                      <xsl:when test = "string(@value)">
                        <td>
                          <xsl:if test="number(@value)">
                            <xsl:attribute name="align">right</xsl:attribute>
                          </xsl:if>
                          <xsl:value-of select="@value"/>
                        </td>
                      </xsl:when >
                      <!--This line is commented since it inserts empty columns for attributes with display == false-->
                      <xsl:otherwise >
                        <td>
                          &#160;
                        </td>
                      </xsl:otherwise>
                    </xsl:choose >
                  </xsl:for-each>

                  <xsl:choose>
                    <xsl:when test="$listType = 'WellBookPageView'">

                      <xsl:call-template name="CreateWellBookViewOptions">
                        <xsl:with-param name="ID" select="$idValue"/>
                        <xsl:with-param name="rowID" select="$rowID"/>
                        <xsl:with-param name ="empty">
                          <xsl:value-of select ="recordInfo/attribute[@name='Empty']/@value"/>
                        </xsl:with-param>
                        <xsl:with-param name ="pagetype">
                          <xsl:value-of select ="recordInfo/attribute[@name='Type']/@value"/>
                        </xsl:with-param>
                        <xsl:with-param name ="signedoff">
                          <xsl:value-of select ="recordInfo/attribute[@name='Signed Off']/@value"/>
                        </xsl:with-param>
                        <xsl:with-param name ="pageurl">
                          <xsl:value-of select ="recordInfo/attribute[@name='Page_URL']/@value"/>
                        </xsl:with-param>
                        <xsl:with-param name ="Chapter_ID">
                          <xsl:value-of select ="recordInfo/attribute[@name='Chapter_ID']/@value"/>
                        </xsl:with-param>
                      </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:call-template name="CreateFuctionOptions">
                        <xsl:with-param name="ID" select="$idValue"/>
                        <xsl:with-param name="rowID" select="$rowID"/>
                      </xsl:call-template>
                    </xsl:otherwise>
                  </xsl:choose>
                  <!-- commment piyush-->
                  <!--<xsl:call-template name="CreateFuctionOptions">
                    <xsl:with-param name="ID" select="$idValue"/>
                  </xsl:call-template>-->
                  <!--</xsl:if>-->
                  <xsl:if test="$addMasterLinkColumn = 'true'">
                    <xsl:call-template name="CreateAddRemoveLink">
                      <xsl:with-param name="ID" select="$idValue"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:if>
                  <xsl:if test="$viewMasterLinkColumn = 'true'">
                    <xsl:call-template name="CreateViewLink">
                      <xsl:with-param name="ID" select="$idValue"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:if>
                </tr>

              </xsl:if>
            </xsl:for-each>
          </tbody>
        </table>
        <!--End of table-->
        <Script language="javascript">
          DWBFixColWidth("tblSearchResults",'<xsl:value-of select="$listType"/>');
        </Script>
      </div>
    </xsl:for-each>

  </xsl:template>
  <!--<xsl:template name="CreateExpandCollapse">
    <xsl:param name="ID"/>

    <xsl:element name="td">
      <xsl:attribute name="ID">
        <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
      </xsl:attribute>
      <xsl:attribute name="style">
        width:"15px";text-align:"center";
      </xsl:attribute>
      <xsl:attribute name="onclick">
        ShowHistoryRow('<xsl:value-of select="$ID"/>','<xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>');
      </xsl:attribute>
      <img src="/_layouts/DREAM/images/info_on.gif" alt="Audit History" />&#160;
    </xsl:element>
  </xsl:template>-->
  <xsl:template name="CreateEditColumn">
    <xsl:param name="ID"/>
    <xsl:element name="td">
      <xsl:attribute name="style">
        width:15px;text-align:center;	cursor:pointer;
      </xsl:attribute>
      <xsl:attribute name="onclick">
        openEditPage('<xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>');
      </xsl:attribute>

      <img src="/_layouts/DREAM/images/EDITICON.GIF" alt="Edit" />&#160;
    </xsl:element>
  </xsl:template>
  <xsl:template name="CreateCheckBoxColumn">
    <xsl:param name="ID"/>
    <xsl:element name="td">
      <xsl:attribute name="class">printerFriendly</xsl:attribute>
      <xsl:attribute name="style">
        width:"5%";text-align:"center";
      </xsl:attribute>
      <xsl:element name="input">
        <xsl:attribute name="type">checkbox</xsl:attribute>
        <xsl:attribute name="name">chbSelectItem</xsl:attribute>
        <xsl:attribute name="id">chbSelectID</xsl:attribute>
        <xsl:attribute name="value">
          <xsl:value-of select="$ID"/>
        </xsl:attribute>
        <xsl:attribute name="onclick">DeselectCheck('chbHeader')</xsl:attribute>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <xsl:template name="CreateAddRemoveLink">
    <xsl:param name="ID"/>
    <xsl:element name="td">
      <xsl:attribute name="style">
        text-align:center;
      </xsl:attribute>
      <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:OpenMasterPageList('</xsl:text>
      <xsl:value-of select="$ID"/> ','change'
      <xsl:text disable-output-escaping="yes">)" style="text-align:center">Add/Remove</xsl:text>
      <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
    </xsl:element>
  </xsl:template>
  <xsl:template name="CreateBookViewerLink">
    <xsl:param name="ID"/>
    <xsl:param name="value"/>
    <xsl:element name="td">
      <xsl:attribute name="style">
        text-align:center;
      </xsl:attribute>
      <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:OpenWellBookViewer('</xsl:text>
      <xsl:value-of select="$ID"/> ',
      <xsl:text disable-output-escaping="yes">)" style="display:inline;"></xsl:text>
      <xsl:value-of select ="$value"/>
      <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
    </xsl:element>
  </xsl:template>
  <xsl:template name ="CreateTerminateLink">
    <xsl:param name ="ID"/>
    <xsl:element name ="td">
      <xsl:attribute name="style">
        text-align:center;&#160;
      </xsl:attribute>
      <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:TerminateListItem('</xsl:text>
      <xsl:value-of select="$ID"/>'
      <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
      <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
    </xsl:element>
  </xsl:template>
  <xsl:template name="CreateViewLink">
    <xsl:param name="ID"/>
    <xsl:element name="td">
      <xsl:attribute name="style">
        text-align:center;
      </xsl:attribute>
      <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:OpenMasterPageList('</xsl:text>
      <xsl:value-of select="$ID"/>','view'
      <xsl:text disable-output-escaping="yes">)" style="display:inline; text-align:center">View</xsl:text>
      <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
    </xsl:element>
  </xsl:template>
  <xsl:template name="CreateFuctionHeaders">
    <xsl:if test="$listType = 'MasterPage'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select ="3"/>
        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if >

    <xsl:if test="$listType = 'Template'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:if test="$activeStatus = 'true'">

          <xsl:attribute name="style">
            text-align:"center";
          </xsl:attribute>
          <xsl:attribute name="colspan">
            <xsl:value-of select ="4"/>
          </xsl:attribute>
          Functions
        </xsl:if>
        <xsl:if test="$activeStatus = 'false'">

          <xsl:attribute name="style">
            text-align:"center";
          </xsl:attribute>
          <xsl:attribute name="colspan">
            <xsl:value-of select ="2"/>
          </xsl:attribute>
          Functions

        </xsl:if>
      </xsl:element>
    </xsl:if >

    <xsl:if test="$listType = 'TemplatePages'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select ="2"/>
        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if >
    <xsl:if test="$listType = 'WellBook'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select ="7"/>

        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if >
    <xsl:if test="$listType = 'BookPages'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select ="3"/>
        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if >
    <xsl:if test="$listType = 'Chapters'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select ="4"/>
        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if >
    <xsl:if test="$listType = 'ChapterPages'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select ="3"/>
        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if >

    <xsl:if test="$listType= 'UserRegistration'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select="2"/>
        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if>

    <xsl:if test="$listType= 'TeamRegistration'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select="3"/>
        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if>

    <xsl:if test="$listType= 'StaffRegistration'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>        
        Functions
      </xsl:element>
    </xsl:if>

    <xsl:if test="$listType = 'WellBookPageView'">
      <xsl:element name="th">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select ="6"/>
        </xsl:attribute>
        Functions
      </xsl:element>
    </xsl:if >

  </xsl:template>

  <xsl:template name="CreateFuctionOptions">
    <xsl:param name="ID"/>
    <xsl:param name="rowID"/>
    <xsl:if test="$listType = 'MasterPage'">
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;	cursor:pointer;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>

      <xsl:element name ="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>

        <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
        <xsl:if test ="$listItemAction='Activate'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
        <xsl:if test ="$listItemAction='Archive'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
      </xsl:element>

      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="ID">
          <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
        </xsl:attribute>
        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
        <xsl:text disable-output-escaping="yes">,1)" style="display:inline;">Audit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>

    </xsl:if >

    <xsl:if test="$listType = 'Template'">

      <xsl:if test="$activeStatus = 'true'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openPages('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Pages</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
        <xsl:element name ="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;
          </xsl:attribute>

          <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
          <xsl:if test ="$listItemAction='Activate'">
            <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
          <xsl:if test ="$listItemAction='Archive'">
            <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
        </xsl:element>
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="ID">
            <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
          </xsl:attribute>
          <xsl:attribute name="style">
            text-align:center;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
          <xsl:text disable-output-escaping="yes">,0)" style="display:inline;">Audit</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if>
      <xsl:if test="$activeStatus = 'false'">
        <xsl:element name ="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
          <xsl:if test ="$listItemAction='Activate'">
            <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
          <xsl:if test ="$listItemAction='Archive'">
            <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
        </xsl:element>
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="ID">
            <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
          </xsl:attribute>
          <xsl:attribute name="style">
            text-align:center;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
          <xsl:text disable-output-escaping="yes">,0)" style="display:inline;">Audit</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if>
    </xsl:if >

    <xsl:if test="$listType = 'TemplatePages'">
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;	cursor:pointer;
        </xsl:attribute>
        <!-- listType = MasterPage since edit function in Template Pages should call the edit screen of the master page-->
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
        <xsl:value-of select="$ID"/>', <xsl:text disable-output-escaping="yes">'TemplateMasterPages'</xsl:text>
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="ID">
          <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
        </xsl:attribute>
        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
        <xsl:text disable-output-escaping="yes">,1)" style="display:inline;">Audit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>

    </xsl:if >
    <xsl:if test="$listType = 'WellBook'">
      <xsl:if test ="$listItemAction='Archive'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if>
      <xsl:if test ="$listItemAction='Archive'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openChapters('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Chapters</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if>
      <xsl:if test ="$listItemAction='Archive'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openBookPages('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Pages</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if>
      <xsl:if test ="$listItemAction='Archive'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>

          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:publishItem('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Publish</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if>
      <xsl:if test ="$listItemAction='Archive'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openChangepageOwner('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Change Page Owner</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if>
      <xsl:element name ="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>

        <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
        <xsl:if test ="$listItemAction='Activate'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
        <xsl:if test ="$listItemAction='Archive'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
      </xsl:element>
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="ID">
          <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
        </xsl:attribute>
        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
        <xsl:text disable-output-escaping="yes">,0)" style="display:inline;">Audit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>

    </xsl:if >
    <xsl:if test="$listType = 'BookPages'">
      <xsl:if test ="$listItemAction='Archive'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if>
      <xsl:element name ="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>

        <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
        <xsl:if test ="$listItemAction='Activate'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
        <xsl:if test ="$listItemAction='Archive'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
      </xsl:element>
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="ID">
          <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
        </xsl:attribute>
        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
        <xsl:text disable-output-escaping="yes">,1)" style="display:inline;">Audit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>

    </xsl:if >
    <xsl:if test="$listType = 'Chapters'">
      <xsl:if test ="$listItemAction='Archive'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openChapterPages('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Pages</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if >
      <xsl:element name ="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>

        <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
        <xsl:if test ="$listItemAction='Activate'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
        <xsl:if test ="$listItemAction='Archive'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
      </xsl:element>
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="ID">
          <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
        </xsl:attribute>
        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
        <xsl:text disable-output-escaping="yes">,1)" style="display:inline;">Audit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>

    </xsl:if >
    <xsl:if test="$listType = 'ChapterPages'">
      <xsl:if test ="$listItemAction='Archive'">
        <xsl:element name="td">
          <xsl:attribute name="class">printerFriendly</xsl:attribute>

          <xsl:attribute name="style">
            text-align:center;	cursor:pointer;
          </xsl:attribute>
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
          <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:element>
      </xsl:if >
      <xsl:element name ="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>

        <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
        <xsl:if test ="$listItemAction='Activate'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
        <xsl:if test ="$listItemAction='Archive'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
      </xsl:element>
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="ID">
          <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
        </xsl:attribute>
        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
        <xsl:text disable-output-escaping="yes">,1)" style="display:inline;">Audit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>

    </xsl:if >

    <xsl:if test="$listType = 'UserRegistration'">
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center; cursor:pointer;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
        <xsl:value-of select="$ID"/>', '<xsl:value-of select="$listType"/>'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>
      <xsl:element name ="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>

        <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
        <xsl:if test ="$listItemAction='Activate'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
        <xsl:if test ="$listItemAction='Archive'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
      </xsl:element>
    </xsl:if>
    <xsl:if test="$listType = 'StaffRegistration'">
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center; cursor:pointer;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
        <xsl:value-of select="$ID"/>', '<xsl:value-of select="$listType"/>'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>
    </xsl:if>
    <xsl:if test="$listType = 'TeamRegistration'">
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center; cursor:pointer;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openEditPage('</xsl:text>
        <xsl:value-of select="$ID"/>', '<xsl:value-of select="$listType"/>'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>


      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;	cursor:pointer;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:openPages('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Staff</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>

      <xsl:element name ="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>

        <xsl:attribute name="style">
          text-align:center;
        </xsl:attribute>

        <xsl:text disable-output-escaping="yes">&lt;a href="#" onclick="Javascript:ShowRemoveOptions('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$listItemAction"/>'
        <xsl:if test ="$listItemAction='Activate'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Activate</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
        <xsl:if test ="$listItemAction='Archive'">
          <xsl:text disable-output-escaping="yes">)" style="display:inline;">Remove</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:if>
      </xsl:element>
    </xsl:if>

  </xsl:template>

  <xsl:template name="Paging">
    <xsl:param name="EndPageNumber"/>
    <xsl:param name="StartPagenumber"/>

    <xsl:if test="$StartPagenumber &lt; $EndPageNumber">
      <xsl:choose>
        <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2)) or $StartPagenumber > ($EndPageNumber - $MaxPages)) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2) or ($StartPagenumber &lt; 1 + $MaxPages))">
          <xsl:if test="$StartPagenumber=$CurrentPage">
            &#160; <b>
              [ <xsl:value-of select="$StartPagenumber + 1"/> ]
            </b> &#160;
          </xsl:if>
          <xsl:if test="$StartPagenumber!=$CurrentPage">
            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
            <xsl:value-of select="$CurrentPageName"/>',
            <xsl:value-of select="$StartPagenumber"/>,
            <xsl:value-of select="$recordCount"/>,
            <xsl:choose>
              <xsl:when test="string($sortBy)">
                '<xsl:value-of select="$sortBy"/>',
              </xsl:when>
              <xsl:otherwise>
                '',
              </xsl:otherwise>
            </xsl:choose>'<xsl:value-of select="$activeStatus"/>',
            '<xsl:value-of select="$sortType"/>')"
            <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="$StartPagenumber+ 1"/>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
        </xsl:when>

        <xsl:when test="$StartPagenumber=0">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>'<xsl:value-of select="$activeStatus"/>',
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;First</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a>&#160;</xsl:text>
        </xsl:when>

        <xsl:when test="($StartPagenumber+1)=$EndPageNumber">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>'<xsl:value-of select="$activeStatus"/>',
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;Last</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>

        <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2) - 1) or $StartPagenumber > ($EndPageNumber - $MaxPages) - 1 ) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2 + 1) or ($StartPagenumber &lt; 2 + $MaxPages))">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>'<xsl:value-of select="$activeStatus"/>',
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;...</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:call-template name="Paging">
        <xsl:with-param name="EndPageNumber" select="$EndPageNumber"/>
        <xsl:with-param name="StartPagenumber" select="$StartPagenumber+1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name="CreateWellBookViewOptions">
    <xsl:param name="ID"/>
    <xsl:param name="empty"/>
    <xsl:param name="pagetype"/>
    <xsl:param name="signedoff"/>
    <xsl:param name="pageurl"/>
    <xsl:param name="Chapter_ID"/>
    <xsl:param name="rowID"/>
    <xsl:if test ="$pagetype != 1">
      <xsl:element name="td">
        <xsl:attribute name="class">printerFriendly</xsl:attribute>
        <xsl:attribute name="style">
          text-align:center;	cursor:pointer;
        </xsl:attribute>
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:UpdatePageContents('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$pagetype"/>'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Update</xsl:text>

        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:element>
    </xsl:if>
    <xsl:if test="$pagetype = 1">
      <xsl:element name="td">
        &#160;
      </xsl:element>
    </xsl:if>
    <xsl:element name="td">
      <xsl:attribute name="class">printerFriendly</xsl:attribute>
      <xsl:attribute name="style">
        text-align:center;	cursor:pointer;
      </xsl:attribute>
      <xsl:if test ="$signedoff = 'No'">
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:SignOffPage('</xsl:text>
        <xsl:value-of select="$ID"/>','PageSignOff'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Sign Off</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:if >
      <xsl:if test ="$signedoff = 'Yes'">
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:SignOffPage('</xsl:text>
        <xsl:value-of select="$ID"/>','CancelPageSignOff'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">Cancel Sign Off</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:if >
    </xsl:element>
    <xsl:element name="td">
      <xsl:attribute name="class">printerFriendly</xsl:attribute>
      <xsl:attribute name="ID">
        <xsl:value-of select="concat('ContainerTableRow' ,$ID)"/>
      </xsl:attribute>
      <xsl:attribute name="style">
        text-align:center;
      </xsl:attribute>
      <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ShowHistoryRow('</xsl:text>
      <xsl:value-of select="$ID"/>','<xsl:value-of select="$AuditListName"/>','<xsl:value-of select="$rowID"/>'
      <xsl:text disable-output-escaping="yes">,3)" style="display:inline;">Audit</xsl:text>
      <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
    </xsl:element>
    <xsl:element name="td">
      <xsl:attribute name="class">printerFriendly</xsl:attribute>
      <xsl:attribute name="style">
        text-align:center;	cursor:pointer;
      </xsl:attribute>
      <xsl:if test ="$empty !='Yes'">
        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:ViewPageContents('</xsl:text>
        <xsl:value-of select="$ID"/>','<xsl:value-of select="$pagetype"/>','<xsl:value-of select="$pageurl"/>','<xsl:value-of select="$Chapter_ID"/>'
        <xsl:text disable-output-escaping="yes">)" style="display:inline;">View</xsl:text>
        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
      </xsl:if>
      <xsl:if test ="$empty !='No'">
        &#160;
      </xsl:if >
    </xsl:element>
  </xsl:template>
  <xsl:template name="PagingForWellBookPageView">
    <xsl:param name="EndPageNumber"/>
    <xsl:param name="StartPagenumber"/>

    <xsl:if test="$StartPagenumber &lt; $EndPageNumber">
      <xsl:choose>
        <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2)) or $StartPagenumber > ($EndPageNumber - $MaxPages)) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2) or ($StartPagenumber &lt; 1 + $MaxPages))">
          <xsl:if test="$StartPagenumber=$CurrentPage">
            &#160; <b>
              [ <xsl:value-of select="$StartPagenumber + 1"/> ]
            </b> &#160;
          </xsl:if>
          <xsl:if test="$StartPagenumber!=$CurrentPage">
            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:doSortAndPaging('</xsl:text>
            <xsl:value-of select="$CurrentPageName"/>',
            <xsl:value-of select="$StartPagenumber"/>,
            <xsl:value-of select="$recordCount"/>,
            <xsl:choose>
              <xsl:when test="string($sortBy)">
                '<xsl:value-of select="$sortBy"/>',
              </xsl:when>
              <xsl:otherwise>
                '',
              </xsl:otherwise>
            </xsl:choose>'<xsl:value-of select="$activeStatus"/>',
            '<xsl:value-of select="$sortType"/>')"
            <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="$StartPagenumber+ 1"/>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
        </xsl:when>

        <xsl:when test="$StartPagenumber=0">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:doSortAndPaging('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>'<xsl:value-of select="$activeStatus"/>',
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;First</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a>&#160;</xsl:text>
        </xsl:when>

        <xsl:when test="($StartPagenumber+1)=$EndPageNumber">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:doSortAndPaging('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>'<xsl:value-of select="$activeStatus"/>',
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;Last</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>

        <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2) - 1) or $StartPagenumber > ($EndPageNumber - $MaxPages) - 1 ) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2 + 1) or ($StartPagenumber &lt; 2 + $MaxPages))">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:doSortAndPaging('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>'<xsl:value-of select="$activeStatus"/>',
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;...</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:call-template name="PagingForWellBookPageView">
        <xsl:with-param name="EndPageNumber" select="$EndPageNumber"/>
        <xsl:with-param name="StartPagenumber" select="$StartPagenumber+1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
