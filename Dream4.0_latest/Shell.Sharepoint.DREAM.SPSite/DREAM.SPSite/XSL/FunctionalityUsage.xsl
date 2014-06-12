<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt">
  <xsl:output method="xml"/>

  <xsl:param name="windowTitle" select="''"/>

  <!--Template to create the Response table.-->
  <xsl:template match="/">
    <div id="tableContainer" class="tableContainer" >
      <!--table to display response data.-->
      <table style="border-right:1px solid #bdbdbd;"  cellpadding="0" cellspacing="0" 
        class="scrollTable" id="tblSearchResults">
        <xsl:for-each select="response/report/record[@recordno=1]">
          <thead class="fixedHeader" id="fixedHeader">
            <tr style="height: 20px;">
              <xsl:for-each select="attribute">
                <th>
                  <xsl:call-template name="ApplyToolTip">
                    <xsl:with-param name="currentNode" select="."></xsl:with-param>
                  </xsl:call-template>
                  <xsl:value-of select="@name"/>&#160;
                  <img style="cursor:hand" class="hidePrintLink" src="/_layouts/dream/images/UP_INACTIVE.GIF" title="Click to Sort" onclick="javascript:StringTableSort('tblSearchResults',this.parentNode)"></img>
                </th>
              </xsl:for-each>
            </tr>
          </thead>
        </xsl:for-each>
        <!--Looping through all record having same report name.-->
        <tbody boder ="1" class="scrollContent">
          <xsl:for-each select="response/report/record">
            <tr height="20px">
              <xsl:for-each select="attribute">
                <xsl:choose>
                  <xsl:when test="@type = 'number'">
                    <td style="text-align:right;width:auto">
                      <xsl:value-of select="format-number(@value, '#0')"/>&#160;&#160;
                    </td>
                  </xsl:when>
                  <xsl:otherwise>
                    <td style="width:auto;">
                      <xsl:value-of select="@value"/>&#160;
                    </td>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:for-each>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>
      <!--End of table-->
    </div>
    <Script language="javascript">
      SetAlternateColorForFUR("tblSearchResults");
      setWindowTitle('<xsl:value-of select="$windowTitle"/>');
    </Script>

  </xsl:template>

  <xsl:template name="ApplyToolTip">
    <xsl:param name="currentNode">
    </xsl:param>
    <xsl:attribute name="title">

      <xsl:if test="string-length($currentNode/@tablename)&gt;0">
        <xsl:text>Table Name: </xsl:text>
        <xsl:value-of select="$currentNode/@tablename"></xsl:value-of>
        <xsl:text>&#10;</xsl:text>
      </xsl:if>

      <xsl:if test="string-length($currentNode/@dbcolumnname)&gt;0">
        <xsl:text>Column Name: </xsl:text>
        <xsl:value-of select="$currentNode/@dbcolumnname"></xsl:value-of>
        <xsl:text>&#10;</xsl:text>
      </xsl:if>

      <xsl:if test="string-length($currentNode/@description)&gt;0">
        <xsl:text>Formula: </xsl:text>
        <xsl:value-of select="$currentNode/@description"></xsl:value-of>
        <xsl:text>&#10;</xsl:text>
      </xsl:if>

    </xsl:attribute>
  </xsl:template>

</xsl:stylesheet>
