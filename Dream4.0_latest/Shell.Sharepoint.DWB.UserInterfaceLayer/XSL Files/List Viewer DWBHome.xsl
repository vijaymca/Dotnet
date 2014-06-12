<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt">
  <xsl:output method="xml"/>
  <!--DWB Required Parameters-->
  <xsl:param name="listType" select="MasterPage"/>

  <!--<xsl:variable name="templateAssetType" select="''"/>-->
  <xsl:param name="activeStatus" select="true"/>
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
  
  <xsl:template match="/">
    <br></br>
    <xsl:variable name="Pages" select="ceiling($recordCount div $recordsPerPage)" />
    <!--To do paging with page numbers-->
    <xsl:variable name="StartPagenumber" select="0"/> 
    <!-- Call Templates of for page number Loop-->
    <xsl:if test="$Pages > 1">
    <xsl:call-template name="Paging">
      <xsl:with-param name="EndPageNumber" select="$Pages"/>
      <xsl:with-param name="StartPagenumber" select="0"/>
    </xsl:call-template>
    <br></br>
    <br></br>
    </xsl:if>
    <xsl:for-each select="records">
      <div id="tableContainer" class="tableContainer">
        <table style="border-right:1px solid #336699;"  cellpadding="4" cellspacing="0" 
          class="scrollTable" id="tblSearchResults" width="99%">
         
            <thead class="fixedHeader" id="fixedHeader">             
              <tr style="height: 20px;">
                  <th>
                    <!--Books-->
                    <xsl:choose>
                      <xsl:when test="$sortBy = 'Name' ">                        
                        <xsl:choose>
                          <xsl:when test="$sortType = 'ascending'">
                            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                            <xsl:value-of select="$CurrentPageName"/>',
                            <xsl:value-of select="$pageNumber"/>,                          
                            <xsl:value-of select="$recordCount"/>,
                            'Name',
                            '<xsl:value-of select="$activeStatus"/>',
                            'descending')"
                            <xsl:text disable-output-escaping="yes">&gt;</xsl:text>Books
                            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                            <xsl:value-of select="$CurrentPageName"/>',
                            <xsl:value-of select="$pageNumber"/>,                          
                            <xsl:value-of select="$recordCount"/>,
                            'Name',
                            '<xsl:value-of select="$activeStatus"/>',
                            'ascending')"
                            <xsl:text disable-output-escaping="yes">&gt;</xsl:text>Books
                            <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                        <xsl:value-of select="$CurrentPageName"/>',
                        <xsl:value-of select="$pageNumber"/>,                       
                        <xsl:value-of select="$recordCount"/>,
                        'Name',
                        '<xsl:value-of select="$activeStatus"/>',
                        '<xsl:value-of select="$sortType"/>')"
                        <xsl:text disable-output-escaping="yes">&gt;</xsl:text>Books
                        <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </th>
                <th>
                  <!--Team-->
                  <xsl:choose>
                    <xsl:when test="$sortBy = 'Team' ">
                      <xsl:choose>
                        <xsl:when test="$sortType = 'ascending'">
                          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                          <xsl:value-of select="$CurrentPageName"/>',
                          <xsl:value-of select="$pageNumber"/>,                         
                          <xsl:value-of select="$recordCount"/>,
                          'Team',
                          '<xsl:value-of select="$activeStatus"/>',
                          'descending')"
                          <xsl:text disable-output-escaping="yes">&gt;</xsl:text>Team
                          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                          <xsl:value-of select="$CurrentPageName"/>',
                          <xsl:value-of select="$pageNumber"/>,                        
                          <xsl:value-of select="$recordCount"/>,
                          'Team',
                          '<xsl:value-of select="$activeStatus"/>',
                          'ascending')"
                          <xsl:text disable-output-escaping="yes">&gt;</xsl:text>Team
                          <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:text disable-output-escaping="yes">&lt;a href="Javascript:eWBPagingAndSorting('</xsl:text>
                      <xsl:value-of select="$CurrentPageName"/>',
                      <xsl:value-of select="$pageNumber"/>,                     
                      <xsl:value-of select="$recordCount"/>,
                      'Team',
                      '<xsl:value-of select="$activeStatus"/>',
                      '<xsl:value-of select="$sortType"/>')"
                      <xsl:text disable-output-escaping="yes">&gt;</xsl:text>Team
                      <xsl:text disable-output-escaping="yes">&lt;/a> </xsl:text>
                    </xsl:otherwise>
                  </xsl:choose>
                </th>
                <th class="printerFriendly">
                  Published<br/>Books               
                </th>
                <th class="printerFriendly">              
                  <xsl:element name="input">
                    <xsl:attribute name="type">checkbox</xsl:attribute>
                    <xsl:attribute name="name">chbHeader</xsl:attribute>
                    <xsl:attribute name="id">chbHeader</xsl:attribute>
                    <xsl:attribute name="onclick">javascript:SelectUnSelectAll(true);</xsl:attribute>
                  </xsl:element>
                </th>
              </tr>
            </thead>

          <tbody border ="1" class="scrollContent">
            <xsl:for-each select="record">
              <xsl:sort select="recordInfo/attribute[@name = $sortBy]/@value" order="{$sortType}" data-type="{$sortDataType}"/>
              <xsl:if test="position() &gt; $recordsPerPage * number($pageNumber) and position()
                    &lt;= number($recordsPerPage * number($pageNumber) + $recordsPerPage)">
                <xsl:variable name="ID">
                  <xsl:value-of select="@recordNumber"/>
                </xsl:variable>

                <xsl:if test="position()  mod 2 = 1">

                  <tr height="20px" style="background-color:#E8E8E8;display:block">

                    <xsl:for-each select="recordInfo">
                      <xsl:if  test ="attribute[@name = 'Name']">
                        <td style="word-wrap:break-word;vertical-align:middle;text-align:left"  width="300">
                          <a href="javascript:void(0)">
                            <xsl:attribute name="onclick">
                              javascript:OpenBookViewer('<xsl:value-of select="$ID"/>');
                            </xsl:attribute>
                            <xsl:value-of select="attribute[@name = 'Name']/@value"/>
                          </a >
                        </td>
                      </xsl:if >

                      <xsl:if  test ="attribute[@name = 'Team']">
                        <td style="word-wrap:break-word;vertical-align:middle;text-align:center"  width="200">
                          <xsl:choose>
                            <xsl:when test = "string(attribute[@name = 'Team']/@value) and (attribute[@name = 'Team']/@display != 'false')">
                              <xsl:value-of select="attribute[@name = 'Team']/@value"/>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose >
                        </td>
                      </xsl:if >

                      <xsl:if  test ="attribute[@name != 'IsFavorite']">
                        <td style="vertical-align:middle;text-align:center" class="printerFriendly">
                          <a href="javascript:void(0)">
                            <xsl:attribute name="onclick">                              
                              javascript:openPublishedBookList('<xsl:value-of select="$ID"/>','<xsl:value-of select="attribute[@name = 'Name']/@value"/>');
                            </xsl:attribute>
                            List
                          </a >
                        </td>
                      </xsl:if >

                      <xsl:if  test ="attribute[@name = 'IsFavorite']">
                        <td style="word-wrap:break-word;vertical-align:middle;text-align:center" width="100" class="printerFriendly">
                          <xsl:element name="input">
                            <xsl:attribute name="type">checkbox</xsl:attribute>
                            <xsl:attribute name="id">chbIsFavorite</xsl:attribute>
                            <xsl:attribute name="onclick">javascript:SelectUnSelectAll(false);</xsl:attribute>
                            <xsl:if test ="attribute[@name = 'IsFavorite']/@value='Yes'">
                              <xsl:attribute name="checked">checked</xsl:attribute>
                              <xsl:attribute name="disabled">disabled</xsl:attribute>
                            </xsl:if>
                            <xsl:attribute name="value">
                              <xsl:value-of select ="$ID"/>
                            </xsl:attribute>
                          </xsl:element>
                        </td>
                      </xsl:if >

                    </xsl:for-each>
                  </tr>
                </xsl:if >

                <xsl:if test="position()  mod 2 = 0">
                  <tr style="display:block" height="20px">
                    <xsl:for-each select="recordInfo">

                      <xsl:if  test ="attribute[@name = 'Name']">
                        <td style="word-wrap:break-word;vertical-align:middle;text-align:left"  width="300">
                          <a href="javascript:void(0)">
                            <xsl:attribute name="onclick">
                              javascript:OpenBookViewer('<xsl:value-of select="$ID"/>');
                            </xsl:attribute>
                            <xsl:value-of select="attribute[@name = 'Name']/@value"/>
                          </a >
                        </td>
                      </xsl:if >

                      <xsl:if  test ="attribute[@name = 'Team']">
                        <td style="word-wrap:break-word;vertical-align:middle;text-align:center"  width="200">
                          <xsl:choose>
                            <xsl:when test = "string(attribute[@name = 'Team']/@value) and (attribute[@name = 'Team']/@display != 'false')">
                              <xsl:value-of select="attribute[@name = 'Team']/@value"/>
                            </xsl:when >
                            <xsl:otherwise >
                              &#160;
                            </xsl:otherwise>
                          </xsl:choose >
                        </td>
                      </xsl:if >

                      <xsl:if  test ="attribute[@name != 'IsFavorite']">
                        <td style="vertical-align:middle;text-align:center" class="printerFriendly">
                          <a href="javascript:void(0)">
                            <xsl:attribute name="onclick">
                              javascript:openPublishedBookList('<xsl:value-of select="$ID"/>','<xsl:value-of select="attribute[@name = 'Name']/@value"/>');
                            </xsl:attribute>
                            List
                          </a >
                        </td>
                      </xsl:if >

                      <xsl:if  test ="attribute[@name = 'IsFavorite']">
                        <td style="word-wrap:break-word;vertical-align:middle;text-align:center" width="100" class="printerFriendly">
                          <xsl:element name="input">
                            <xsl:attribute name="type">checkbox</xsl:attribute>
                            <xsl:attribute name="id">chbIsFavorite</xsl:attribute>
                            <xsl:attribute name="onclick">javascript:SelectUnSelectAll(false);</xsl:attribute>
                            <xsl:if test ="attribute[@name = 'IsFavorite']/@value='Yes'">                             
                              <xsl:attribute name="checked">checked</xsl:attribute>
                              <xsl:attribute name="disabled">disabled</xsl:attribute>
                            </xsl:if>
                            <xsl:attribute name="value">
                              <xsl:value-of select ="$ID"/>
                            </xsl:attribute>
                          </xsl:element>
                        </td>
                      </xsl:if >

                    </xsl:for-each>
                  </tr >
                </xsl:if >
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
</xsl:stylesheet>
