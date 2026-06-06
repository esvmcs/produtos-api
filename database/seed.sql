-- Seed data for Pyferium Produtos API
-- Database: mydb_local
-- Warning: this script resets sample data.

USE `mydb_local`;

SET FOREIGN_KEY_CHECKS = 0;

DELETE FROM `GEN_PRODUTO`;
DELETE FROM `GEN_CATEGORIA`;

ALTER TABLE `GEN_PRODUTO` AUTO_INCREMENT = 1;
ALTER TABLE `GEN_CATEGORIA` AUTO_INCREMENT = 1;

SET FOREIGN_KEY_CHECKS = 1;

INSERT INTO `GEN_CATEGORIA`
    (`CODCATEGORIA`, `DSCCATEGORIA`, `CODNIVEL`, `IDTATIVO`)
VALUES
    (1, 'Informática', '01', 'S'),
    (2, 'Eletrônicos', '02', 'S'),
    (3, 'Periféricos', '03', 'S'),
    (4, 'Acessórios', '04', 'S'),
    (5, 'Categoria Inativa', '05', 'N');

INSERT INTO `GEN_PRODUTO`
    (`CODPRODUTO`, `NOMPRODUTO`, `CODCATEGORIA`, `VLRPRODUTO`, `IDTATIVO`)
VALUES
    (1, 'Notebook Dell Inspiron', 1, 3899.90, 'S'),
    (2, 'Monitor LG 24 Polegadas', 2, 899.90, 'S'),
    (3, 'Teclado Mecânico Redragon', 3, 249.90, 'S'),
    (4, 'Mouse Logitech M170', 3, 79.90, 'S'),
    (5, 'Headset HyperX Cloud Stinger', 3, 299.90, 'S'),
    (6, 'Suporte para Notebook', 4, 119.90, 'S'),
    (7, 'Produto Inativo de Exemplo', 4, 99.90, 'N');