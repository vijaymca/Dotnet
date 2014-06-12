<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt">
  <xsl:output method="xml"/>

  <xsl:param name="listType" select="MasterPage"/>
  <xsl:param name="editColumn" select="false"/>
  <xsl:param name="TeamPermission" select="''"/>
  <xsl:variable name="idValue" select="''"/>
  <!-- Set the number of records per page-->
  <xsl:param name="recordsPerPage" select="0" />
  <!-- Page Number field -->
  <xsl:param name="pageNumber" select="1" />
  <!--start value-->
  <!--<xsl:param name="startValue" select="0"/>-->
  <!--end value-->
  <!--<xsl:param name="endValue" select="0"/>-->
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

  <xsl:template match="/">
    <br/><br/>
    <xsl:variable name="Pages" select="ceiling($recordCount div $recordsPerPage)" />
    <!--To do paging with page numbers-->
    <xsl:variable name="StartPagenumber" select="0"/>
    <!-- Call Templates of for page number Loop-->
    <xsl:call-template name="Paging">
      <xsl:with-param name="EndPageNumber" select="$Pages"/>
      <xsl:with-param name="StartPagenumber" select="0"/>
    </xsl:call-template>

    <br/>
    <br/>
    <xsl:for-each select="records">
      <div id="tableContainer" class="tableContainer">
        <table style="border-right:1px solid #BDBDBD;"  cellpadding="0" cellspacing="0" 
          class="scrollTable" id="tblSearchResults">
          <xsl:for-each select="record[@order=1]">
            <thead class="fixedHeader" id="fixedHeader">
              <tr style="height: 20px;">

                <xsl:for-each select="recordInfo/attribute[@display != 'false']">
                  <th>
                    <xsl:choose>
                      <xsl:when test="$sortBy = @name ">
                        <xsl:choose>
                          <xsl:when test="$sortType = 'ascending'">
                            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPagingListReport('</xsl:text>
                            <xsl:value-of select="$CurrentPageName"/>',
                            <xsl:value-of select="$pageNumber"/>,
                            <!--<xsl:value-of select="$StartPagenumber - 1"/>,-->
                            <xsl:value-of select="$recordCount"/>,
                            '<xsl:value-of select="@name"/>',
                            'descending')"
                            <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                          </xsl:when>
                          <xsl:when test="$sortType = 'descending'">
                            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPagingListReport('</xsl:text>
                            <xsl:value-of select="$CurrentPageName"/>',
                            <xsl:value-of select="$pageNumber"/>,
                            <!--<xsl:value-of select="$StartPagenumber - 1"/>,-->
                            <xsl:value-of select="$recordCount"/>,
                            '<xsl:value-of select="@name"/>',
                            'ascending')"
                            <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPagingListReport('</xsl:text>
                            <xsl:value-of select="$CurrentPageName"/>',
                            <xsl:value-of select="$pageNumber"/>,
                            <!--<xsl:value-of select="$StartPagenumber - 1"/>,-->
                            <xsl:value-of select="$recordCount"/>,
                            '<xsl:value-of select="@name"/>',
                            'descending')"
                            <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPagingListReport('</xsl:text>
                        <xsl:value-of select="$CurrentPageName"/>',
                        <xsl:value-of select="$pageNumber"/>,
                        <!--<xsl:value-of select="$StartPagenumber - 1"/>,-->
                        <xsl:value-of select="$recordCount"/>,
                        '<xsl:value-of select="@name"/>',
                        '<xsl:value-of select="$sortType"/>')"
                        <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="@name"/>
                        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </th>
                </xsl:for-each>
                <xsl:call-template name ="CreateFuctionHeaders" />
              </tr>
            </thead>
          </xsl:for-each>
          <tbody border ="1" class="scrollContent">
            <xsl:for-each select="record">
              <xsl:sort select="recordInfo/attribute[@name = $sortBy]/@value" order="{$sortType}" data-type="{$sortDataType}"/>
              <xsl:if test="position() &gt; $recordsPerPage * number($pageNumber) and position()
                    &lt;= number($recordsPerPage * number($pageNumber) + $recordsPerPage)">
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

                  <xsl:for-each select="recordInfo/attribute[@display != 'false']">
                    <xsl:if test="$listType= 'TeamRegistration'">
                      <xsl:choose>
                        <xsl:when test = "string(@value)">
                          <td style="word-wrap:break-word" width="330">
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
                    </xsl:if>
                  </xsl:for-each>
           
                      <xsl:call-template name="CreateFuctionOptions">
                        <xsl:with-param name="ID" select="$idValue"/>
                        <xsl:with-param name="permission" select="$TeamPermission"/>
                      </xsl:call-template>
                </tr>
              </xsl:if >
            </xsl:for-each>
          </tbody>
        </table>
        <!--End of table-->
        <Script language="javascript">
          ListReportFixColWidth("tblSearchResults",'<xsl:value-of select="$listType"/>','<xsl:value-of select="$TeamPermission"/>');
        </Script>
      </div>
    </xsl:for-each>

  </xsl:template>


  <xsl:template name="CreateFuctionHeaders">
    <xsl:element name="th">
      
      <xsl:attribute name="class">printerFriendly</xsl:attribute>
      <xsl:if test="$listType= 'TeamRegistration'">
        <xsl:attribute name="style">
          text-align:"center";
        </xsl:attribute>
        <xsl:attribute name="colspan">
          <xsl:value-of select="3"/>
        </xsl:attribute>
        Functions
    </xsl:if>
      
    </xsl:element>
  </xsl:template>

  <xsl:template name="CreateFuctionOptions">
    <xsl:param name="ID"/>
    <xsl:param name="permission"/>
    
    <xsl:if test="$listType = 'TeamRegistration'">


      <xsl:choose >
        <xsl:when  test ="$permission='DreamAdmin'">
          <xsl:element name="td">
            <xsl:attribute name="class">printerFriendly</xsl:attribute>

            <xsl:attribute name="style">
              text-align:center; cursor:pointer;
            </xsl:attribute>
            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:OpenPages('</xsl:text>
            <xsl:value-of select="$ID"/>', '<xsl:value-of select="$listType"/>','editteam'
            <xsl:text disable-output-escaping="yes">)" style="display:inline;">Edit</xsl:text>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:element>

          <xsl:element name="td">
            <xsl:attribute name="class">printerFriendly</xsl:attribute>

            <xsl:attribute name="style">
              text-align:center;	cursor:pointer;
            </xsl:attribute>
            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:OpenPages('</xsl:text>
            <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>','managestaff'
            <xsl:text disable-output-escaping="yes">)" style="display:inline;">Staff</xsl:text>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:element>

          <xsl:element name="td">
            <xsl:attribute name="class">printerFriendly</xsl:attribute>

            <xsl:attribute name="style">
              text-align:center;	cursor:pointer;
            </xsl:attribute>

            <a href="#" style="display:inline;">
              <xsl:attribute name="onclick">
                javascript:return DeleteRecord('<xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>');
              </xsl:attribute>
              Delete
            </a>
          </xsl:element>
        </xsl:when>

        <xsl:when  test ="$permission='TeamOwner'">
     
          <xsl:element name="td">
            <xsl:attribute name="class">printerFriendly</xsl:attribute>
            <xsl:attribute name="style">
              text-align:center;	cursor:pointer;
            </xsl:attribute>
            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:OpenPages('</xsl:text>
            <xsl:value-of select="$ID"/>','<xsl:value-of select="$listType"/>','managestaff'
            <xsl:text disable-output-escaping="yes">)" style="display:inline;">Staff</xsl:text>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:element>
        
        </xsl:when>

        <xsl:when  test ="$permission='NonRegUser'">

          <xsl:element name="td">
            <xsl:attribute name="class">printerFriendly</xsl:attribute>
            <xsl:attribute name="style">
              text-align:center;	cursor:pointer;
            </xsl:attribute>

            <a href="#" style="display:inline;">
              <xsl:attribute name="onclick">
                javascript:return OpenSameWindow('/_layouts/DREAM/AccessApproval.aspx?teamId=<xsl:value-of select="$ID"/>');
              </xsl:attribute>
              Request Access
            </a>
            
          </xsl:element>

        </xsl:when>
      </xsl:choose>
      
      
    

    </xsl:if>

  </xsl:template>

  <xsl:template name="Paging">
    <xsl:param name="EndPageNumber"/>
    <xsl:param name="StartPagenumber"/>
    <xsl:if test="$StartPagenumber &lt; $EndPageNumber">
      <xsl:choose>
        <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2)) or $StartPagenumber > ($EndPageNumber - $MaxPages)) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2) or ($StartPagenumber &lt; 1 + $MaxPages))">
          <!--<xsl:value-of select="$CurrentPage"/>
          <xsl:value-of select="$StartPagenumber"/>-->
          <xsl:if test="$StartPagenumber=$CurrentPage">
            &#160; <b>
              [ <xsl:value-of select="$StartPagenumber + 1"/> ]
            </b> &#160;
          </xsl:if>
          <xsl:if test="$StartPagenumber!=$CurrentPage">
            <!--<xsl:text disable-output-escaping="yes">&lt;a href="Javascript:alert('hi');"</xsl:text>-->
            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPagingListReport('</xsl:text>
            <xsl:value-of select="$CurrentPageName"/>',
            <!--<xsl:value-of select="$StartPagenumber"/>,-->
            <xsl:value-of select="$StartPagenumber"/>,
            <xsl:value-of select="$recordCount"/>,
            <xsl:choose>
              <xsl:when test="string($sortBy)">
                '<xsl:value-of select="$sortBy"/>',
              </xsl:when>
              <xsl:otherwise>
                '',
              </xsl:otherwise>
            </xsl:choose>
            '<xsl:value-of select="$sortType"/>')"
            <xsl:text disable-output-escaping="yes">&gt;</xsl:text><xsl:value-of select="$StartPagenumber+ 1"/>
            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
          </xsl:if>
        </xsl:when>

        <xsl:when test="$StartPagenumber=0">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPagingListReport('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <!--<xsl:value-of select="$StartPagenumber"/>,-->
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;First</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a>&#160;</xsl:text>
        </xsl:when>

        <xsl:when test="($StartPagenumber+1)=$EndPageNumber">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPagingListReport('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <!--<xsl:value-of select="$StartPagenumber"/>,-->
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;Last</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>

        <xsl:when test="($StartPagenumber > ($CurrentPage - ceiling($MaxPages div 2) - 1) or $StartPagenumber > ($EndPageNumber - $MaxPages) - 1 ) and (($StartPagenumber &lt; $CurrentPage + $MaxPages div 2 + 1) or ($StartPagenumber &lt; 2 + $MaxPages))">
          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:QuickSearchPagingListReport('</xsl:text>
          <xsl:value-of select="$CurrentPageName"/>',
          <!--<xsl:value-of select="$StartPagenumber"/>,-->
          <xsl:value-of select="$StartPagenumber"/>,
          <xsl:value-of select="$recordCount"/>,
          <xsl:choose>
            <xsl:when test="string($sortBy)">
              '<xsl:value-of select="$sortBy"/>',
            </xsl:when>
            <xsl:otherwise>
              '',
            </xsl:otherwise>
          </xsl:choose>,
          '<xsl:value-of select="$sortType"/>')"
          <xsl:text disable-output-escaping="yes">&gt;...</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
        </xsl:when>
      </xsl:choose>
      <xsl:call-template name="Paging">
        <xsl:with-param name="EndPageNumber" select="$EndPageNumber"/>
        <xsl:with-param name="StartPagenumber" select="$StartPagenumber+1"/>
        <!--<xsl:with-param name="StartPagenumber" select="$StartPagenumber"/>-->
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
