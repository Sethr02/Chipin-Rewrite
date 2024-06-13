-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: localhost    Database: chipin_db
-- ------------------------------------------------------
-- Server version	8.0.33

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
-- Table structure for table `address`
--

DROP TABLE IF EXISTS `address`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `address` (
  `address_id` int NOT NULL AUTO_INCREMENT,
  `adress_name` varchar(255) DEFAULT NULL,
  `address_1` varchar(255) DEFAULT NULL,
  `address_2` varchar(255) DEFAULT NULL,
  `city` varchar(255) DEFAULT NULL,
  `state` varchar(255) DEFAULT NULL,
  `country` varchar(255) DEFAULT NULL,
  `post_code` varchar(255) DEFAULT NULL,
  `is_default` tinyint(1) DEFAULT NULL,
  `first_name` varchar(255) DEFAULT NULL,
  `last_name` varchar(255) DEFAULT NULL,
  `phone_number` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `chipin_id` varchar(128) NOT NULL,
  PRIMARY KEY (`address_id`),
  KEY `fk_address_user_table1_idx` (`chipin_id`),
  CONSTRAINT `fk_address_user_table1` FOREIGN KEY (`chipin_id`) REFERENCES `user_table` (`chipin_id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `address`
--

LOCK TABLES `address` WRITE;
/*!40000 ALTER TABLE `address` DISABLE KEYS */;
INSERT INTO `address` VALUES (1,'21 Bayview road, Links','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',5),(2,'21 Bayview road, Links','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',8),(3,'21 Bayview road, Links','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',10),(6,'21 Bayview road, Links','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',24),(7,'asd','asd','asd',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,10),(8,'8735 Sherwood Dr. Woodside','8735 Sherwood Dr. Woodside','Links','Somerset West','Western Cape','United States','11377',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',24),(9,'8735 Sherwood Dr. Woodside','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',24),(10,'8735 Sherwood Dr. Woodside','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',24),(11,'asd','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',24),(12,'21 Bayview road, Links','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',1,'Alfred','Rademan','0826167459','alfred@markitevolution.com',24),(13,'New Address','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',0,'Alfred','Rademan','0826167459','alfred@markitevolution.com',24);
/*!40000 ALTER TABLE `address` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `billing_address`
--

DROP TABLE IF EXISTS `billing_address`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `billing_address` (
  `billing_address_id` int NOT NULL AUTO_INCREMENT,
  `adress_name` varchar(255) DEFAULT NULL,
  `address_1` varchar(255) DEFAULT NULL,
  `address_2` varchar(255) DEFAULT NULL,
  `city` varchar(255) DEFAULT NULL,
  `state` varchar(255) DEFAULT NULL,
  `country` varchar(255) DEFAULT NULL,
  `post_code` varchar(255) DEFAULT NULL,
  `is_default` tinyint(1) DEFAULT NULL,
  `chipin_id` varchar(128) NOT NULL,
  `first_name` varchar(255) DEFAULT NULL,
  `last_name` varchar(255) DEFAULT NULL,
  `phone_number` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`billing_address_id`),
  KEY `chipin_id_idx` (`chipin_id`),
  CONSTRAINT `fk_chipin_id.usertable` FOREIGN KEY (`chipin_id`) REFERENCES `user_table` (`chipin_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `billing_address`
--

LOCK TABLES `billing_address` WRITE;
/*!40000 ALTER TABLE `billing_address` DISABLE KEYS */;
INSERT INTO `billing_address` VALUES (8,'21 Bayview road, Links','21 Bayview road, Links','Links','Somerset West','Western Cape','South Africa','7130',1,24,'Alfred','Rademan','0826167459','alfred@markitevolution.com');
/*!40000 ALTER TABLE `billing_address` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `external_products`
--

DROP TABLE IF EXISTS `external_products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `external_products` (
  `external_product_id` int NOT NULL AUTO_INCREMENT,
  `product_name` varchar(255) DEFAULT NULL,
  `product_description` text,
  `price` float DEFAULT NULL,
  `page_url` text,
  `store` varchar(255) DEFAULT NULL,
  `cust_string1` varchar(255) DEFAULT NULL,
  `cust_string2` varchar(255) DEFAULT NULL,
  `cust_string3` varchar(255) DEFAULT NULL,
  `cust_int1` int DEFAULT NULL,
  `cust_int2` int DEFAULT NULL,
  `cust_int3` int DEFAULT NULL,
  `image` text,
  `image1` text,
  `image2` text,
  `image3` text,
  `return_url` text,
  `product_id` text NOT NULL,
  `quantity` int DEFAULT NULL,
  PRIMARY KEY (`external_product_id`)
) ENGINE=InnoDB AUTO_INCREMENT=109 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `external_products`
--

LOCK TABLES `external_products` WRITE;
/*!40000 ALTER TABLE `external_products` DISABLE KEYS */;
INSERT INTO `external_products` VALUES (28,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(29,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(30,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(31,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(32,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(33,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(34,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(35,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(36,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(37,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(38,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(39,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(40,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(41,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(42,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(43,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(44,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(45,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(46,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(47,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(48,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(49,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(50,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(51,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(52,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(53,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(54,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(55,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(56,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(57,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(58,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(59,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(60,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','',NULL),(61,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(62,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(63,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(64,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(65,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(66,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(67,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(68,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(69,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(70,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(71,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(72,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(73,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(74,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(75,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(76,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(77,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(78,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(79,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(80,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(81,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(82,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(83,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(84,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(85,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(86,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(87,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(88,'Fossil Rectangular Sunglasses Dark Havana','<h5><a href=\"https://upgrade.branddeals.co.za/fullsite/product/fossil-rectangular-sunglasses-dark-havana/\"><strong>Fossil Rectangular Sunglasses Dark Havana</strong></a></h5>\r\nTechnical Specifications\r\n\r\nBrand: Fossil\r\nGender: Women\r\nYear: 2021\r\nFrame Colour: Dark Havana\r\nLens Colour: Gradient Brown\r\nFrame Shape: Square\r\nFrame Style: Full Rim\r\nFrame Material: Injected Plastic\r\nLens Material: Plastic\r\nUPC: 716736350684\r\nManufacturer: Safilo\r\nUV Protection: Category 3',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(89,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(90,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(91,'Fossil Rectangular Sunglasses Dark Havana','<h5><a href=\"https://upgrade.branddeals.co.za/fullsite/product/fossil-rectangular-sunglasses-dark-havana/\"><strong>Fossil Rectangular Sunglasses Dark Havana</strong></a></h5>\r\nTechnical Specifications\r\n\r\nBrand: Fossil\r\nGender: Women\r\nYear: 2021\r\nFrame Colour: Dark Havana\r\nLens Colour: Gradient Brown\r\nFrame Shape: Square\r\nFrame Style: Full Rim\r\nFrame Material: Injected Plastic\r\nLens Material: Plastic\r\nUPC: 716736350684\r\nManufacturer: Safilo\r\nUV Protection: Category 3',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(92,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(93,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(94,'Fossil Rectangular Sunglasses Dark Havana','<h5><a href=\"https://upgrade.branddeals.co.za/fullsite/product/fossil-rectangular-sunglasses-dark-havana/\"><strong>Fossil Rectangular Sunglasses Dark Havana</strong></a></h5>\r\nTechnical Specifications\r\n\r\nBrand: Fossil\r\nGender: Women\r\nYear: 2021\r\nFrame Colour: Dark Havana\r\nLens Colour: Gradient Brown\r\nFrame Shape: Square\r\nFrame Style: Full Rim\r\nFrame Material: Injected Plastic\r\nLens Material: Plastic\r\nUPC: 716736350684\r\nManufacturer: Safilo\r\nUV Protection: Category 3',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(95,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(96,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',NULL),(97,'Fossil Rectangular Sunglasses Dark Havana','Test Product description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(98,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(99,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(100,'Fossil Rectangular Sunglasses Dark Havana','Test Product description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(101,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(102,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(103,'Fossil Rectangular Sunglasses Dark Havana','Test Product description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(104,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(105,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(106,'Fossil Rectangular Sunglasses Dark Havana','Test Product description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',699,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(107,'Polaroid Mens Rectangular Sunglasses Black','Test Product Description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',629,NULL,'Brand Deals','CustString1',NULL,NULL,22824,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2023/04/020_2-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1),(108,'Fossil Rectangular Sunglasses Dark Havana','Test Product Description. This is just a filler. The actual product description contains html in the woocomerce database. No idea why they did that',299,NULL,'Brand Deals','CustString1',NULL,NULL,12634,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-content/uploads/2022/08/Untitled-design-jpg-600x366.webp',NULL,NULL,NULL,'https://upgrade.branddeals.co.za/fullsite/wp-json/custom-order-plugin/v1/receive-data','12',1);
/*!40000 ALTER TABLE `external_products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `password_reset`
--

DROP TABLE IF EXISTS `password_reset`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `password_reset` (
  `password_reset_id` int NOT NULL AUTO_INCREMENT,
  `chipin_id` varchar(128) NOT NULL,
  `token` mediumtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` datetime NOT NULL,
  `email` mediumtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `salt` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`password_reset_id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `password_reset`
--

LOCK TABLES `password_reset` WRITE;
/*!40000 ALTER TABLE `password_reset` DISABLE KEYS */;
/*!40000 ALTER TABLE `password_reset` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product_list_items`
--

DROP TABLE IF EXISTS `product_list_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_list_items` (
  `chipin_product_list_entry_id` int NOT NULL AUTO_INCREMENT,
  `product_id` int DEFAULT NULL,
  `quantity` int DEFAULT NULL,
  `product_list_wallet_id` int NOT NULL,
  `external_product_id` int DEFAULT NULL,
  PRIMARY KEY (`chipin_product_list_entry_id`),
  KEY `fk_product_list_products1_idx` (`product_id`),
  KEY `fk_product_list_items_product_list_wallet1_idx` (`product_list_wallet_id`),
  KEY `fk_product_list_items_external_products1_idx` (`external_product_id`),
  CONSTRAINT `fk_product_list_items_external_products1` FOREIGN KEY (`external_product_id`) REFERENCES `external_products` (`external_product_id`) ON DELETE CASCADE,
  CONSTRAINT `fk_product_list_items_product_list_wallet1` FOREIGN KEY (`product_list_wallet_id`) REFERENCES `product_list_wallet` (`product_list_wallet_id`) ON DELETE CASCADE,
  CONSTRAINT `fk_product_list_items_products1` FOREIGN KEY (`product_id`) REFERENCES `products` (`product_id`)
) ENGINE=InnoDB AUTO_INCREMENT=116 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_list_items`
--

LOCK TABLES `product_list_items` WRITE;
/*!40000 ALTER TABLE `product_list_items` DISABLE KEYS */;
INSERT INTO `product_list_items` VALUES (32,NULL,1,13,28),(33,NULL,1,13,29),(34,NULL,1,13,30),(35,NULL,1,13,31),(36,NULL,1,13,32),(37,NULL,1,13,33),(38,3,3,15,NULL),(39,NULL,1,16,34),(40,NULL,1,16,35),(41,NULL,1,16,36),(66,NULL,NULL,32,61),(67,NULL,NULL,32,62),(68,NULL,NULL,32,63),(69,NULL,NULL,33,64),(70,NULL,NULL,33,65),(71,NULL,NULL,33,66),(72,NULL,NULL,34,67),(73,NULL,NULL,34,68),(74,NULL,NULL,34,69),(75,NULL,NULL,35,70),(76,NULL,NULL,35,71),(77,NULL,NULL,35,72),(78,NULL,NULL,37,73),(79,NULL,NULL,37,74),(80,NULL,NULL,37,75),(81,NULL,NULL,38,76),(82,NULL,NULL,38,77),(83,NULL,NULL,38,78),(91,NULL,NULL,39,85),(92,NULL,NULL,39,86),(97,NULL,NULL,22,91),(98,NULL,NULL,22,92),(99,NULL,NULL,22,93),(103,NULL,1,41,97),(104,NULL,1,41,98),(105,NULL,1,41,99),(113,NULL,1,45,106),(114,NULL,1,45,107),(115,NULL,1,45,108);
/*!40000 ALTER TABLE `product_list_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product_list_wallet`
--

DROP TABLE IF EXISTS `product_list_wallet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_list_wallet` (
  `product_list_wallet_id` int NOT NULL AUTO_INCREMENT,
  `total` float DEFAULT NULL,
  `funded` float DEFAULT NULL,
  `name` varchar(45) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `end_at` datetime DEFAULT NULL,
  `chipin_id` varchar(128) NOT NULL,
  `address_id` int DEFAULT NULL,
  `closed` tinyint DEFAULT NULL,
  PRIMARY KEY (`product_list_wallet_id`),
  KEY `fk_product_list_wallet_user_table1_idx` (`chipin_id`),
  KEY `fk_product_list_wallet_address1_idx` (`address_id`),
  CONSTRAINT `fk_product_list_wallet_address1` FOREIGN KEY (`address_id`) REFERENCES `address` (`address_id`),
  CONSTRAINT `fk_product_list_wallet_user_table1` FOREIGN KEY (`chipin_id`) REFERENCES `user_table` (`chipin_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_list_wallet`
--

LOCK TABLES `product_list_wallet` WRITE;
/*!40000 ALTER TABLE `product_list_wallet` DISABLE KEYS */;
INSERT INTO `product_list_wallet` VALUES (13,NULL,NULL,'Test List',NULL,NULL,'2023-08-16 10:40:00',5,NULL,NULL),(15,66,0,'Test List','2023-08-16 11:12:36','2023-08-16 11:12:36','2023-08-17 13:12:00',8,2,NULL),(16,NULL,NULL,'Test List',NULL,NULL,'2023-08-18 17:35:00',9,NULL,NULL),(22,0,0,'Test1','2023-08-23 10:31:55','2023-08-23 10:31:55','2023-08-24 00:00:00',10,3,NULL),(23,0,0,'List11231','2023-08-23 10:37:53','2023-08-23 10:37:53','2023-08-25 00:00:00',10,NULL,NULL),(24,0,0,'List11231','2023-08-23 10:44:09','2023-08-23 10:44:09','2023-08-25 00:00:00',10,NULL,NULL),(25,0,0,'List11231','2023-08-23 10:44:32','2023-08-23 10:44:32','2023-08-25 00:00:00',10,NULL,NULL),(26,0,0,'11111','2023-08-23 10:45:09','2023-08-23 10:45:09','2023-08-24 00:00:00',10,NULL,NULL),(27,0,0,'11111','2023-08-23 10:46:25','2023-08-23 10:46:25','2023-08-24 00:00:00',10,NULL,NULL),(28,0,0,'3333','2023-08-23 10:48:21','2023-08-23 10:48:21','2023-08-24 00:00:00',10,NULL,NULL),(29,0,0,'aaaaaaa','2023-08-23 18:48:20','2023-08-23 18:48:20','2023-08-24 00:00:00',10,NULL,NULL),(30,0,0,'aaaaaaa','2023-08-23 19:15:56','2023-08-23 19:15:56','2023-08-24 00:00:00',10,NULL,NULL),(31,0,0,'qwe','2023-08-23 19:24:20','2023-08-23 19:24:20','2023-08-25 00:00:00',10,NULL,NULL),(32,0,0,'aaaaa','2023-08-23 19:28:56','2023-08-23 19:28:56','2023-08-24 00:00:00',10,NULL,NULL),(33,0,0,'aww','2023-08-24 13:11:42','2023-08-24 13:11:42','2023-08-31 00:00:00',10,NULL,NULL),(34,0,0,'aa111','2023-08-24 13:34:31','2023-08-24 13:34:31','2023-08-31 00:00:00',10,NULL,NULL),(35,0,0,'qweqweqwe','2023-08-24 16:46:00','2023-08-24 16:46:00','2023-09-01 00:00:00',10,NULL,NULL),(36,0,0,'99','2023-08-27 19:34:34','2023-08-27 19:34:34','2023-08-29 00:00:00',10,NULL,NULL),(37,0,0,'99','2023-08-27 19:37:27','2023-08-27 19:37:27','2023-08-29 00:00:00',10,NULL,NULL),(38,0,0,'999','2023-08-27 19:37:53','2023-08-27 19:37:53','2023-08-29 00:00:00',10,NULL,NULL),(39,NULL,0,'new list','2023-09-06 17:17:27','2023-09-06 17:17:27','2023-09-07 00:00:00',10,3,NULL),(40,0,0,'brandhub order','2023-09-06 18:22:32','2023-09-06 18:22:32','2023-09-28 00:00:00',10,NULL,NULL),(41,0,0,'List1','2023-09-06 18:29:34','2023-09-06 18:29:34','2023-09-15 00:00:00',10,NULL,NULL),(45,1627,0,'Test List','2023-09-07 10:26:00','2023-09-07 10:26:00','2023-09-14 00:00:00',24,13,NULL);
/*!40000 ALTER TABLE `product_list_wallet` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product_list_wallet_transaction`
--

DROP TABLE IF EXISTS `product_list_wallet_transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_list_wallet_transaction` (
  `product_list_wallet_transaction_id` int NOT NULL AUTO_INCREMENT,
  `amount` float DEFAULT NULL,
  `chipin_id` varchar(128) DEFAULT NULL,
  `from_invited_user` tinyint DEFAULT NULL,
  `product_list_wallet_id` int NOT NULL,
  `transaction_method` varchar(255) NOT NULL,
  PRIMARY KEY (`product_list_wallet_transaction_id`),
  KEY `fk_product_list_wallet_transaction_product_list_wallet1_idx` (`product_list_wallet_id`),
  CONSTRAINT `fk_product_list_wallet_transaction_product_list_wallet1` FOREIGN KEY (`product_list_wallet_id`) REFERENCES `product_list_wallet` (`product_list_wallet_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_list_wallet_transaction`
--

LOCK TABLES `product_list_wallet_transaction` WRITE;
/*!40000 ALTER TABLE `product_list_wallet_transaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `product_list_wallet_transaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `product_id` int NOT NULL AUTO_INCREMENT,
  `product_name` varchar(255) DEFAULT NULL,
  `product_description` text,
  `store` varchar(255) DEFAULT NULL,
  `product_image` text,
  `quantity` int DEFAULT NULL,
  `price` float DEFAULT NULL,
  `product_image1` text,
  `product_image2` text,
  `product_image3` text,
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (3,'beans','I am beans','Watermelon store','https://www.recipetineats.com/wp-content/uploads/2014/05/Homemade-Heinz-Baked-Beans_0-SQ.jpg',4,22,NULL,NULL,NULL);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salt`
--

DROP TABLE IF EXISTS `salt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salt` (
  `salt_id` int NOT NULL AUTO_INCREMENT,
  `chipin_id` varchar(128) DEFAULT NULL,
  `salt` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`salt_id`),
  KEY `fk_salt_user_table1_idx` (`chipin_id`),
  CONSTRAINT `fk_salt_user_table1` FOREIGN KEY (`chipin_id`) REFERENCES `user_table` (`chipin_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salt`
--

LOCK TABLES `salt` WRITE;
/*!40000 ALTER TABLE `salt` DISABLE KEYS */;
/*!40000 ALTER TABLE `salt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `share_list`
--

DROP TABLE IF EXISTS `share_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `share_list` (
  `share_id` int NOT NULL AUTO_INCREMENT,
  `share_name` varchar(255) DEFAULT NULL,
  `share_email` varchar(255) DEFAULT NULL,
  `share_message` text,
  `share_date` datetime DEFAULT NULL,
  `product_list_wallet_transaction_id` int NOT NULL,
  PRIMARY KEY (`share_id`),
  KEY `fk_share_list_product_list_wallet_transaction1_idx` (`product_list_wallet_transaction_id`),
  CONSTRAINT `fk_share_list_product_list_wallet_transaction1` FOREIGN KEY (`product_list_wallet_transaction_id`) REFERENCES `product_list_wallet_transaction` (`product_list_wallet_transaction_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `share_list`
--

LOCK TABLES `share_list` WRITE;
/*!40000 ALTER TABLE `share_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `share_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `signature_keys`
--

DROP TABLE IF EXISTS `signature_keys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `signature_keys` (
  `store_id` varchar(45) NOT NULL,
  `store_key` varchar(45) NOT NULL,
  PRIMARY KEY (`store_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `signature_keys`
--

LOCK TABLES `signature_keys` WRITE;
/*!40000 ALTER TABLE `signature_keys` DISABLE KEYS */;
/*!40000 ALTER TABLE `signature_keys` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `temp_user_table`
--

DROP TABLE IF EXISTS `temp_user_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `temp_user_table` (
  `chipin_id` varchar(128) NOT NULL UNIQUE ,
  `chipin_name` varchar(45) DEFAULT NULL,
  `chipin_created_date` datetime DEFAULT NULL,
  `token_wallet_id` int DEFAULT NULL,
  `user_email` varchar(255) DEFAULT NULL,
  `user_pass` varchar(255) DEFAULT NULL,
  `salt` varchar(255) DEFAULT NULL,
  `verified` tinyint(1) DEFAULT '0',
  `token` text,
  `created_at` datetime DEFAULT NULL,
  PRIMARY KEY (`chipin_id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `temp_user_table`
--

LOCK TABLES `temp_user_table` WRITE;
/*!40000 ALTER TABLE `temp_user_table` DISABLE KEYS */;
/*!40000 ALTER TABLE `temp_user_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `token_wallet`
--

DROP TABLE IF EXISTS `token_wallet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `token_wallet` (
  `token_wallet_id` int NOT NULL AUTO_INCREMENT,
  `amount` float DEFAULT NULL,
  `chipin_id` varchar(128) NOT NULL,
  PRIMARY KEY (`token_wallet_id`),
  KEY `fk_token_wallet_user_table1_idx` (`chipin_id`),
  CONSTRAINT `fk_token_wallet_user_table1` FOREIGN KEY (`chipin_id`) REFERENCES `user_table` (`chipin_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `token_wallet`
--

LOCK TABLES `token_wallet` WRITE;
/*!40000 ALTER TABLE `token_wallet` DISABLE KEYS */;
INSERT INTO `token_wallet` VALUES (1,1,5),(5,0,28),(6,0,29),(7,0,30);
/*!40000 ALTER TABLE `token_wallet` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `urls`
--

DROP TABLE IF EXISTS `urls`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `urls` (
  `url_id` int NOT NULL AUTO_INCREMENT,
  `product_list_wallet_id` int NOT NULL,
  `url` text COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`url_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `urls`
--

LOCK TABLES `urls` WRITE;
/*!40000 ALTER TABLE `urls` DISABLE KEYS */;
/*!40000 ALTER TABLE `urls` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_table`
--

DROP TABLE IF EXISTS `user_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_table` (
  `chipin_id` varchar(128) NOT NULL UNIQUE,
  `chipin_name` varchar(45) DEFAULT NULL,
  `chipin_created_date` datetime DEFAULT NULL,
  `token_wallet_id` int DEFAULT NULL,
  `user_email` varchar(255) DEFAULT NULL,
  `user_pass` varchar(255) DEFAULT NULL,
  `salt` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`chipin_id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_table`
--

LOCK TABLES `user_table` WRITE;
/*!40000 ALTER TABLE `user_table` DISABLE KEYS */;
INSERT INTO `user_table` VALUES ('5','test@test.com','2023-08-15 08:11:35',966460556,'test@test.com','RPdiDfcvN2/6vqTYLwjmas/83derfJtxbgdwU2kVNdk=','FLW1dKjssMSFOhCs'),('6','test@test.com','2023-08-15 09:10:28',1066528637,'test@test.com','p14IONJ+5qMGvT6XsDEzKN3Yv1sSvHRKK6JCela7AOI=','NBErqK1fFCez0Nxq'),('7','test@test.com','2023-08-15 09:10:44',-1273238956,'test@test.com','x6Heoyft/CyyjXc+vAl7BlH1Zgpko8LVDmHuca5z9gA=','0gqRth5xa0lIPPRi'),('8','A@A.A','2023-08-15 09:10:55',1296418282,'A@A.A','NNth2+HutjJb50pKQA60cwhR7bfdMZSRmwA7B4DVTpI=','C7F7jh28JN3NPia2'),('9','AAAA@AAAA.com','2023-08-17 15:23:33',1326465992,'AAAA@AAAA.com','FH1XVSq4gHBEYNEuwqxdNliLavBQbHyFgLooPVB2u2w=','tbR3SWUOk726C4xc'),('10','testing@test.com','2023-08-20 20:28:02',828629003,'testing@test.com','sRknMLuTkN8iRXzhMAJsyDdh3CQIG8ZeXIe3+05i6dY=','vgp6yN8qvS7NNtur'),('11','aAAA','2023-08-28 08:24:16',784892638,'aAAA','aPY3qfGs3pSzY4irZPqbiQC++9rUrL2l2sOX0jBGBgI=','YtOWnlCDnxU3DDvX'),('12','asda','2023-08-28 08:26:18',-1213907269,'asda','8ijG4OwoeNseFlIChW2UG2Cq0RbB2Wao5mxr4q7ViLI=','EMryToHojUSNtflm'),('13','ASD@ASD.com','2023-08-28 08:31:28',1831968224,'ASD@ASD.com','+A5P4snGFdi9BUFwSUxJnJFpBnstyVSSUKX/d5hnkpo=','kaIAYdvH7qt7iSST'),('14','testing@test.co','2023-08-28 11:05:35',-1802138481,'testing@test.co','LravC9gcr43SPKHZ3cxts5RQq2zRlcKQJTAVheCEIVo=','2kOo9MaCWO85FGTh'),('15','testing@test.co3','2023-08-28 11:36:52',551814371,'testing@test.co3','xUcMp5e4vReUipaXfMjHfzbTrMg+fJ631tYVhExzWkI=','r9C9hK8hMhDgdAUV'),(16,'asdasd3@as','2023-08-28 13:00:25',449134284,'asdasd3@as','Y6nrEfBAEy6D1QUjyiklopa44uUD2+1eeEORw2c19Cw=','FE6ZgtK39TIlf3sX'),(17,'tt@tt.com','2023-09-04 20:19:25',849139325,'tt@tt.com','c+F5iEim6jHMw7YR1gim3fnzsL57KALHoiyUgzzCOJU=','Qv5lxlHtxUreaqM7'),('24','rademanalfred@gmail.com','2023-09-07 10:24:58',2143274578,'rademanalfred@gmail.com','HNGMSdOqSWj7v/yxjySVkt1wIkRwCAwHatEvp5rA4KE=','Ynp5yETljbckw180'),('28',NULL,NULL,NULL,'asdad','qweasd','asdasd'),('29',NULL,NULL,NULL,'asdad','qweasd','asdasd'),(30,NULL,NULL,NULL,'asdad','qweasd','asdasd');
/*!40000 ALTER TABLE `user_table` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-09-12 10:53:59