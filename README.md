# FbfhDataExtrator
國際貿易局廠商資料的萃取器
![ScreenShot](https://github.com/stanChung/FbfhDataExtrator/blob/master/FbfhDataExtrator_H1.PNG)
![ScreenShot](https://github.com/stanChung/FbfhDataExtrator/blob/master/FbfhDataExtrator_H2.PNG)
![ScreenShot](https://github.com/stanChung/FbfhDataExtrator/blob/master/FbfhDataExtrator_H3.PNG)
![ScreenShot](https://github.com/stanChung/FbfhDataExtrator/blob/master/FbfhDataExtrator_H4.PNG)

###**功能描述**
>- ASP.NET+Html Agility Pack+Oracle
>- 仿照國際貿易局原本的查詢操作製作WEB頁面，但實際上是以WebClient去連接國際貿易局的原頁面，取得對方Server回應後的HTML Document加以萃取出所需要的廠商資料。
>- 在查詢列表加上勾選介面，將選取的廠商資料一併匯入資料庫。

###**Tablespace建立**
```sql
--tablespace SQL
CREATE SMALLFILE TABLESPACE FBFH 
    DATAFILE 
        'C:\APP\ORACLE_OP\ORADATA\ORADEV01\FBFH.DBF' SIZE 1321861120 AUTOEXTEND ON NEXT 655360 MAXSIZE 34359721984 
    BLOCKSIZE 8192 
    NOLOGGING 
    DEFAULT NOCOMPRESS NO INMEMORY 
    ONLINE 
    SEGMENT SPACE MANAGEMENT AUTO 
    EXTENT MANAGEMENT LOCAL AUTOALLOCATE;
    
    
```

###**使用者建立**
```sql
-- USER SQL
CREATE USER FBFH IDENTIFIED BY 1qaz2wsx 
DEFAULT TABLESPACE "FBFH"
TEMPORARY TABLESPACE "TEMP";

-- QUOTAS
ALTER USER FBFH QUOTA UNLIMITED ON FBFH;

-- ROLES
GRANT "CONNECT" TO FBFH ;

-- SYSTEM PRIVILEGES
GRANT CREATE SESSION TO FBFH ;


```
###**資料表建立**
```sql
--------------------------------------------------------
--  DDL for Table FBFH_VENDER
--------------------------------------------------------

  CREATE TABLE "FBFH"."FBFH_VENDER" 
   (    "COMPANY_UNI" NVARCHAR2(20), 
    "ORIGINAL_REGISTERED_DATE" DATE, 
    "APPROVAL_DATE" DATE, 
    "COMPANY_CNAME" NVARCHAR2(100), 
    "COMPANY_ENAME" NVARCHAR2(100), 
    "COMPANY_CADDRESS" NVARCHAR2(300), 
    "COMPANY_EADDRESS" NVARCHAR2(300), 
    "COMPANY_OWNER" NVARCHAR2(20), 
    "COMPANY_TEL_1" NVARCHAR2(20), 
    "COMPANY_TEL_2" NVARCHAR2(20), 
    "COMPANY_FAX" NVARCHAR2(20), 
    "COMPANY_ORIGINAL_CNAME" NVARCHAR2(100), 
    "COMPANY_ORIGNAL_ENAME" NVARCHAR2(100), 
    "IMPORT_QUALIFICATION" CHAR(1 BYTE), 
    "EXPORT_QUALIFICATION" CHAR(1 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "FBFH" ;

   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_UNI" IS '統一編號';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."ORIGINAL_REGISTERED_DATE" IS '原始登記日期';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."APPROVAL_DATE" IS '核發日期';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_CNAME" IS '廠商中文名稱';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_ENAME" IS '廠商英文名稱';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_CADDRESS" IS '中文營業地址';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_EADDRESS" IS '英文營業地址';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_OWNER" IS '代表人';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_TEL_1" IS '電話1';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_TEL_2" IS '電話2';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_FAX" IS '傳真';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_ORIGINAL_CNAME" IS '廠商原中文名稱';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."COMPANY_ORIGNAL_ENAME" IS '廠商原英文名稱';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."IMPORT_QUALIFICATION" IS '進口資格';
   COMMENT ON COLUMN "FBFH"."FBFH_VENDER"."EXPORT_QUALIFICATION" IS '出口資格';
--------------------------------------------------------
--  DDL for Index FBFH_VENDER_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "FBFH"."FBFH_VENDER_PK" ON "FBFH"."FBFH_VENDER" ("COMPANY_UNI", "COMPANY_EADDRESS", "COMPANY_CNAME") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "FBFH" ;
--------------------------------------------------------
--  Constraints for Table FBFH_VENDER
--------------------------------------------------------

  ALTER TABLE "FBFH"."FBFH_VENDER" ADD CONSTRAINT "FBFH_VENDER_PK" PRIMARY KEY ("COMPANY_UNI", "COMPANY_EADDRESS", "COMPANY_CNAME")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "FBFH"  ENABLE;
  ALTER TABLE "FBFH"."FBFH_VENDER" MODIFY ("COMPANY_EADDRESS" NOT NULL ENABLE);
  ALTER TABLE "FBFH"."FBFH_VENDER" MODIFY ("COMPANY_CNAME" NOT NULL ENABLE);
  ALTER TABLE "FBFH"."FBFH_VENDER" MODIFY ("ORIGINAL_REGISTERED_DATE" NOT NULL ENABLE);
  ALTER TABLE "FBFH"."FBFH_VENDER" MODIFY ("COMPANY_UNI" NOT NULL ENABLE);

```
