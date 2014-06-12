<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  
  
  <xsl:param name ="PARAM1" select="''"></xsl:param>
  <xsl:param name ="PARAM2" select="''"></xsl:param>
  <xsl:param name ="PARAM3" select="''"></xsl:param>
  <xsl:param name ="PARAM4" select="''"></xsl:param>
  <xsl:param name ="PARAM5" select="''"></xsl:param>
  <xsl:param name ="PARAM6" select="''"></xsl:param>
  <xsl:param name ="PARAM7" select="''"></xsl:param>

  <xsl:variable name="NEWPARAM1">
    <xsl:choose >
      <xsl:when test ="$PARAM1 = 'Pick Name'">PickName</xsl:when>
      <xsl:when test ="$PARAM1 = 'Owner ID'">OwnerID</xsl:when>
      <xsl:when test ="$PARAM1 = 'Asset Owner'">AssetOwner</xsl:when>
      <xsl:when test ="$PARAM1 = 'EC Code'">Code</xsl:when>
      <xsl:when test ="$PARAM1 = 'Status Value'">StatusValue</xsl:when>
      <xsl:when test ="$PARAM1 = 'Field Name'">FieldName</xsl:when>
      <xsl:when test ="$PARAM1 = 'Lithostrat Group'">LithostratGroup</xsl:when>
      <xsl:when test ="$PARAM1 = 'Lithostrat Formation'">LithostratFormation</xsl:when>
      <xsl:when test ="$PARAM1 = 'Lithostrat Member'">LithostratMember</xsl:when>
      <xsl:when test ="$PARAM1 = 'Basin Name'">Basin</xsl:when>
      <xsl:otherwise >
        <xsl:value-of select ="$PARAM1"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  
  <xsl:variable name="NEWPARAM2">
    <xsl:choose >
      <xsl:when test ="$PARAM2 = 'AHBDF AH'">AHBDFAH</xsl:when>
      <xsl:when test ="$PARAM2 = 'Owner Name'">OwnerName</xsl:when>
      <xsl:when test ="$PARAM2 = 'SWED'">SWEDCode</xsl:when>
      <xsl:when test ="$PARAM2 = 'Resource Description'">ResourceDescription</xsl:when>
      <xsl:when test ="$PARAM2 = 'Field Identifier'">FieldValue</xsl:when>
      <xsl:otherwise >
        <xsl:value-of select ="$PARAM2"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="NEWPARAM3">
    <xsl:choose >
      <xsl:when test ="$PARAM3 = 'Owner Name'">OwnerName</xsl:when>
      <xsl:otherwise >
        <xsl:value-of select ="$PARAM3"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="NEWPARAM4">
    <xsl:choose >
      <xsl:when test ="$PARAM4 = 'Owner Name'">OwnerName</xsl:when>
      <xsl:otherwise >
        <xsl:value-of select ="$PARAM4"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="NEWPARAM5">
    <xsl:choose >
      <xsl:when test ="$PARAM5 = 'Owner Name'">OwnerName</xsl:when>
      <xsl:otherwise >
        <xsl:value-of select ="$PARAM5"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:variable name="NEWPARAM6">
        <xsl:value-of select ="$PARAM6"/>
  </xsl:variable>


  <xsl:variable name="NEWPARAM7">
    <xsl:value-of select ="$PARAM7"/>
  </xsl:variable>
  <!--Copy all nodes-->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

  <!--Delete recordcount node-->
  <xsl:template match ="recordcount">
    
  </xsl:template>

  <!--Copy all record nodes-->
  <xsl:template match="record">
   
    <xsl:copy>
        <xsl:apply-templates select="@*"/>
        <attribute>
          <xsl:attribute name ="{$NEWPARAM1}">
            <xsl:value-of select ="current()/self::node()/attribute[@name=$PARAM1]/@value"/>
          </xsl:attribute>
          <xsl:if test ="string($NEWPARAM2)">
            <xsl:attribute name ="{$NEWPARAM2}">
              <xsl:value-of select ="current()/self::node()/attribute[@name=$PARAM2]/@value"/>
            </xsl:attribute>
          </xsl:if >
          <xsl:if test ="string($NEWPARAM3)">
            <xsl:attribute name ="{$NEWPARAM3}">
              <xsl:value-of select ="current()/self::node()/attribute[@name=$PARAM3]/@value"/>
            </xsl:attribute>
          </xsl:if >
          <xsl:if test ="string($NEWPARAM4)">
            <xsl:attribute name ="{$NEWPARAM4}">
              <xsl:value-of select ="current()/self::node()/attribute[@name=$PARAM4]/@value"/>
            </xsl:attribute>
          </xsl:if >
          <xsl:if test ="string($NEWPARAM5)">
            <xsl:attribute name ="{$NEWPARAM5}">
              <xsl:value-of select ="current()/self::node()/attribute[@name=$PARAM5]/@value"/>
            </xsl:attribute>
          </xsl:if >
          <xsl:if test ="string($NEWPARAM6)">
            <xsl:attribute name ="{$NEWPARAM6}">
              <xsl:value-of select ="current()/self::node()/attribute[@name=$PARAM6]/@value"/>
            </xsl:attribute>
          </xsl:if >
          <xsl:if test ="string($NEWPARAM7)">
            <xsl:attribute name ="{$NEWPARAM7}">
              <xsl:value-of select ="current()/self::node()/attribute[@name=$PARAM7]/@value"/>
            </xsl:attribute>
          </xsl:if >
        </attribute>
      </xsl:copy>
    </xsl:template>

  </xsl:stylesheet>
