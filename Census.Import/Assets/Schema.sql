-- MySQL dump 10.13  Distrib 8.0.21, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: census
-- ------------------------------------------------------
-- Server version	8.0.21

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `act`
--

DROP TABLE IF EXISTS `act`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `act` (
  `id` int NOT NULL AUTO_INCREMENT,
  `bookId` int NOT NULL,
  `typeId` int NOT NULL,
  `subtypeId` int NOT NULL,
  `familyId` int DEFAULT NULL,
  `companyId` int DEFAULT NULL,
  `placeId` int DEFAULT NULL,
  `label` varchar(200) NOT NULL,
  `note` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `family_act_idx` (`familyId`),
  KEY `company_act_idx` (`companyId`),
  KEY `book_act_idx` (`bookId`),
  KEY `place_act_idx` (`placeId`),
  KEY `type_act_idx` (`typeId`),
  KEY `subtype_act_idx` (`subtypeId`),
  CONSTRAINT `book_act` FOREIGN KEY (`bookId`) REFERENCES `book` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `company_act` FOREIGN KEY (`companyId`) REFERENCES `company` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `family_act` FOREIGN KEY (`familyId`) REFERENCES `family` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `place_act` FOREIGN KEY (`placeId`) REFERENCES `place` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `subtype_act` FOREIGN KEY (`subtypeId`) REFERENCES `actSubtype` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `type_act` FOREIGN KEY (`typeId`) REFERENCES `actType` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=261 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `actCategory`
--

DROP TABLE IF EXISTS `actCategory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actCategory` (
  `actId` int NOT NULL,
  `categoryId` int NOT NULL,
  `unsure` bit(1) NOT NULL,
  PRIMARY KEY (`actId`,`categoryId`),
  KEY `category_ac_idx` (`categoryId`),
  CONSTRAINT `act_ac` FOREIGN KEY (`actId`) REFERENCES `act` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `category_ac` FOREIGN KEY (`categoryId`) REFERENCES `category` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `actPartner`
--

DROP TABLE IF EXISTS `actPartner`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actPartner` (
  `actId` int NOT NULL,
  `partnerId` int NOT NULL,
  PRIMARY KEY (`actId`,`partnerId`),
  KEY `person_ap_idx` (`partnerId`),
  CONSTRAINT `act_ap` FOREIGN KEY (`actId`) REFERENCES `act` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `person_ap` FOREIGN KEY (`partnerId`) REFERENCES `person` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `actProfession`
--

DROP TABLE IF EXISTS `actProfession`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actProfession` (
  `actId` int NOT NULL,
  `professionId` int NOT NULL,
  `unsure` bit(1) NOT NULL,
  PRIMARY KEY (`actId`,`professionId`),
  KEY `profession_ap_idx` (`professionId`),
  CONSTRAINT `act_apf` FOREIGN KEY (`actId`) REFERENCES `act` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `profession_apf` FOREIGN KEY (`professionId`) REFERENCES `profession` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `actSubtype`
--

DROP TABLE IF EXISTS `actSubtype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actSubtype` (
  `id` int NOT NULL AUTO_INCREMENT,
  `actTypeId` int NOT NULL,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `type_subtype_idx` (`actTypeId`),
  CONSTRAINT `type_subtype` FOREIGN KEY (`actTypeId`) REFERENCES `actType` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `actType`
--

DROP TABLE IF EXISTS `actType`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `actType` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `archive`
--

DROP TABLE IF EXISTS `archive`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `archive` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `book`
--

DROP TABLE IF EXISTS `book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `book` (
  `id` int NOT NULL AUTO_INCREMENT,
  `archiveId` int NOT NULL,
  `typeId` int NOT NULL,
  `subtypeId` int NOT NULL,
  `writePlaceId` int DEFAULT NULL,
  `writerId` int DEFAULT NULL,
  `location` varchar(100) DEFAULT NULL,
  `description` varchar(1000) DEFAULT NULL,
  `incipit` varchar(3000) DEFAULT NULL,
  `startYear` smallint NOT NULL,
  `endYear` smallint NOT NULL,
  `edition` varchar(500) DEFAULT NULL,
  `note` varchar(3000) DEFAULT NULL,
  `file` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `archive_book_idx` (`archiveId`),
  KEY `place_book_idx` (`writePlaceId`),
  KEY `person_book_idx` (`writerId`),
  KEY `booktype_book_idx` (`typeId`),
  KEY `booksubtype_book_idx` (`subtypeId`),
  CONSTRAINT `archive_book` FOREIGN KEY (`archiveId`) REFERENCES `archive` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `booksubtype_book` FOREIGN KEY (`subtypeId`) REFERENCES `bookSubtype` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `booktype_book` FOREIGN KEY (`typeId`) REFERENCES `bookType` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `person_book` FOREIGN KEY (`writerId`) REFERENCES `person` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `place_book` FOREIGN KEY (`writePlaceId`) REFERENCES `place` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=193 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bookSubtype`
--

DROP TABLE IF EXISTS `bookSubtype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bookSubtype` (
  `id` int NOT NULL AUTO_INCREMENT,
  `bookTypeId` int NOT NULL,
  `name` varchar(200) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `booktype_booksubtype_idx` (`bookTypeId`),
  CONSTRAINT `booktype_booksubtype` FOREIGN KEY (`bookTypeId`) REFERENCES `bookType` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=79 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bookType`
--

DROP TABLE IF EXISTS `bookType`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bookType` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `category` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `company`
--

DROP TABLE IF EXISTS `company`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `company` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `previousId` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `family`
--

DROP TABLE IF EXISTS `family`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `family` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `person`
--

DROP TABLE IF EXISTS `person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `place`
--

DROP TABLE IF EXISTS `place`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `place` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `profession`
--

DROP TABLE IF EXISTS `profession`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profession` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-10-08 18:40:28
