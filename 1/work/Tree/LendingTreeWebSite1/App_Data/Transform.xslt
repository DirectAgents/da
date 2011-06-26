<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="data">
    <data>
      <xsl:apply-templates select="options" />
    </data>
  </xsl:template>
  <xsl:template match="options">
    <options>
      <xsl:attribute name="question">
        <xsl:value-of select="@question" />
      </xsl:attribute>
      <xsl:apply-templates select="option" />
    </options>
  </xsl:template>
  <xsl:template match="option">
    <choice>
      <xsl:attribute name="text">
        <xsl:value-of select="." />
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:value-of select="@value" />
      </xsl:attribute>
    </choice>
  </xsl:template>
</xsl:stylesheet>
