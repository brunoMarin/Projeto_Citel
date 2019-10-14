/* =================================================
|				PROVA TÉCNICA - CITEL				|
----------------------------------------------------
|	Autor: Bruno Marin Rodrigues					|
|	Data: 11/10/2019								|
|	Descrição: Script banco de dados				|
|  	Banco de dados: MySQL							|
====================================================|  
|													|
|				INSTRUÇÕES							|
----------------------------------------------------|
| 1) Executar o script abaixo para gerar o banco	|
|	 de dados, as tabelas e as stored procedures.	|
|													|
| 2) Alterar a connection string nas duas classes:	|
|	* CategorialModel								|
|	* ProdutolModel									|
|													|
| 3) Dar "Clean" e depois Build no ProjetoCitel		|
|													|
====================================================*/


/*CRIAR BANCO DE DADOS DBCitel*/
CREATE DATABASE DBCitel;


/*CRIAR TABELAS
CATEGORIA*/

CREATE TABLE DBCitel.tblCategoria
(
	IdCategoria INT NOT NULL AUTO_INCREMENT,
	DcCategoria VARCHAR(50),
    PRIMARY KEY (IdCategoria)
);

/*PRODUTO*/
CREATE TABLE DBCitel.tblProduto
(
	IdProduto INT NOT NULL AUTO_INCREMENT,
	IdCategoria INT ,
	DcProduto VARCHAR(50),
	VlrPreco DECIMAL(10,2),
	DcCaracteristica VARCHAR(200),
    PRIMARY KEY (IdProduto),
	CONSTRAINT fk_CatProduto FOREIGN KEY (IdCategoria) REFERENCES tblCategoria (IdCategoria)
);


/*		PROCEDURES		*/

/*		INSERIR CATEGORIA		*/
DELIMITER $$
CREATE PROCEDURE dbcitel.uspInsertCategoria(in _DcCategoria VARCHAR(50))
BEGIN
		
	DECLARE ultimoRegistro int;
    SET ultimoRegistro = (SELECT LAST_INSERT_ID());

	INSERT INTO dbcitel.tblCategoria 
		(DcCategoria) 
	VALUES 
        (_DcCategoria);
	
     IF(ultimoRegistro < (SELECT LAST_INSERT_ID()))
     THEN 
		SELECT 'OK';
	 ELSE
		SELECT 'ERRO';
	END IF;
     
END$$
DELIMITER ;

/*		SELECT CATEGORIA		*/
CALL DELIMITER $$
CREATE  PROCEDURE dbcitel.uspSelectCategoria()
BEGIN

	SELECT
		IdCategoria,
		DcCategoria
	FROM 
		dbcitel.tblCategoria;
     
END$$
DELIMITER ;

/*		ALTERAR CATEGORIA		*/
DELIMITER $$
CREATE PROCEDURE dbcitel.uspUpdateCategoria (in _IdCategoria INT, in _DcCategoria VARCHAR(50))
BEGIN

	UPDATE
		DBCitel.tblCategoria 
	  SET 
        DcCategoria = _DcCategoria 
	 WHERE 
		IdCategoria = _IdCategoria;
     
	SELECT 'OK';
     
END$$
DELIMITER ;

/*		DELETAR CATEGORIA		*/
DELIMITER $$
CREATE PROCEDURE dbcitel.uspDeleteCategoria (in _IdCategoria INT)
BEGIN

	DELETE FROM
		DBCitel.tblCategoria 
	 WHERE 
		IdCategoria = _IdCategoria;
     
	SELECT 'OK';
     
END$$
DELIMITER ;

/*		INSERIR PRODUTO		*/
DELIMITER $$
CREATE PROCEDURE dbcitel.uspInsertProduto (in _IdCategoria INT, in _DcProduto varchar(50), in _VlrPreco decimal(10,2), in _DcCaracteristica varchar(200))
BEGIN
		
	DECLARE ultimoRegistro int;
    SET ultimoRegistro = (SELECT LAST_INSERT_ID());
    
	INSERT INTO
		dbcitel.tblProduto
	(
		IdCategoria	
        ,DcProduto	
        ,VlrPreco	
        ,DcCaracteristica
    )
    VALUES
	(
		_IdCategoria	
		,_DcProduto	
		,_VlrPreco	
		,_DcCaracteristica
	);
    
   IF(ultimoRegistro < (SELECT LAST_INSERT_ID()))
     THEN 
		SELECT 'OK';
	 ELSE
		SELECT 'ERRO';
	END IF;
     
END$$
DELIMITER ;

/*		SELECT PRODUTO */
DELIMITER $$
CREATE PROCEDURE DBCitel.uspSelectProduto()
BEGIN

	SELECT
		tblProduto.IdCategoria,
		DcCategoria,
		IdProduto,
        DcProduto,
		VlrPreco,
        DcCaracteristica
	FROM 
		dbcitel.tblProduto
	LEFT JOIN
		dbcitel.tblCategoria ON tblCategoria.IdCategoria = tblProduto.IdCategoria ;
     
END $$
DELIMITER ;]


/*		ALTERAR PRODUTO		*/
DELIMITER $$
CREATE PROCEDURE dbcitel.uspUpdateProduto (in _IdProduto INT, in _IdCategoria INT, in _DcProduto varchar(50), in _VlrPreco decimal(10,2), in _DcCaracteristica varchar(200))
BEGIN

	UPDATE
		dbcitel.tblProduto
	  SET 
        IdCategoria			= _IdCategoria
        ,DcProduto			= _DcProduto
        ,VlrPreco			= _VlrPreco
        ,DcCaracteristica	= _DcCaracteristica
	 WHERE 
		IdProduto = _IdProduto;
     
	SELECT 'OK';
     
END$$
DELIMITER ;

/*		DELETAR PRODUTO		*/
DELIMITER $$
CREATE PROCEDURE dbcitel.uspDeleteProduto(in _IdProduto INT)
BEGIN

	DELETE FROM
		DBCitel.tblProduto
	 WHERE 
		IdProduto = _IdProduto;
     
	SELECT 'OK';
     
END$$
DELIMITER ;

/*		SELECIONAR PRODUTO	FILTRO	*/
DELIMITER $$
CREATE PROCEDURE dbcitel.uspSelectProdutoFiltro(in _IdCategoria INT)
BEGIN

	SELECT
		tblProduto.IdCategoria,
		DcCategoria,
		IdProduto,
        DcProduto,
		VlrPreco,
        DcCaracteristica
	FROM 
		dbcitel.tblProduto
	LEFT JOIN
		dbcitel.tblCategoria ON tblCategoria.IdCategoria = tblProduto.IdCategoria 
	WHERE
		tblProduto.IdCategoria = _IdCategoria;
     
END$$
DELIMITER ;

