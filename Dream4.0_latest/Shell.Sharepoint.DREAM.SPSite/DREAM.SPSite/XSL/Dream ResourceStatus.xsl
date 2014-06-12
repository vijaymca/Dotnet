<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- Template to create Response Table-->
  <xsl:param name="Flag" select="0" />
<xsl:template match="/">
    <html>
    <body> 
    <!--
        This is an XSLT template file. Fill in this area with the
        XSL elements which will transform your XML to XHTML.
    -->
      <!--Looping through each record-->
      <div id="tableContainer" class="tableContainer">
        <!--table to display response data.-->
       <!-- <h3>Resource Status</h3>-->
        <table height="98%" style="border-top:1px solid #BDBDBD;"  cellpadding="0" cellspacing="0" 
          class="scrollTable" id="tblSResourceStatus">
          <!--table headers of the response data.-->
          <thead class="fixedHeader" id="fixedHeader">
            <tr style="height: 20px;">
              <th width="5%" text-align="center">Resource Name</th>
              <th width="3%" text-align="center">Status</th>
              <xsl:for-each select="response/resources/resource">
                <xsl:if test="@value='false'">
                  <xsl:variable name="Flag" select="1" />
                </xsl:if>
                </xsl:for-each>
              <xsl:if test="@Flag = '1'">
                <th width="10%" text-align="center">Description</th>
              </xsl:if>
            </tr>
          </thead >
          <xsl:for-each select="response/resources/resource">
            <tbody class="scrollContent">
              <tr  style="height: 20px">
                <td width="5%" text-align="center">
                  <xsl:value-of select="@name"/>
                </td>
                <td width="3%" text-align="center">
                  <xsl:if test="@value='true'">
                    <img src="/_layouts/DREAM/images/ewr212l.gif" alt="Available" />
                  </xsl:if>
                  <xsl:if test="@value='false'">
                    <img src="/_layouts/DREAM/images/ewr213l.gif" alt="Unavailable" />
                  </xsl:if>
                </td>
                <xsl:if test="@value = 'false'">
                  <td width="10%" text-align="center">
                    <xsl:value-of select="@desc"/>
                  </td>
                </xsl:if>
              </tr>
            </tbody >
          </xsl:for-each >
        </table >
        <!-- End table-->
      </div>
    </body>
    </html>
</xsl:template>
</xsl:stylesheet> 
