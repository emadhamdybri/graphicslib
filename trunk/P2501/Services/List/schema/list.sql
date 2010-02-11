
SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";

--
-- Table structure for table `list`
--

CREATE TABLE IF NOT EXISTS `list` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Hostname` tinytext,
  `IP` tinytext,
  `HostPort` int(11) DEFAULT NULL,
  `ServerName` tinytext,
  `Description` tinytext,
  `Groupname` tinytext,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=6 ;
