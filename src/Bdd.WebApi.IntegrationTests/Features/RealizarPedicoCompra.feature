Feature: Realizar Pedido
    Eu como Cliente
    Gostaria de realizar pedido


Scenario: total acima do valor minimo
    Given configuracao com valor minimo de 50
    And os seguintes produtos no pedido
        | sku | quantidade | preco_unitario |
        | 123 |         10 |         1.99 |
        | 321 |          2 |        15.99 |
    When pedido realizado
    Then pedido deve ser concluido

Scenario: total abaixo do valor minimo
    Given configuracao com valor minimo de 50
    And os seguintes produtos no pedido
        | sku | quantidade | preco_unitario |
        | 123 |         10 |         1.99 |
        | 321 |          1 |        15.99 |
    When pedido realizado
    Then pedido NAO deve ser concluido

Scenario: validacao do estoque do produto
    Given os estoques com os seguintes produtos:
        | sku | estoque |
        | 123 |      10 |
        | 321 |       1 |
        | 456 |       5 |
        | 654 |      20 |
    When solicitado o produto <sku> com quantidade <quantidade>
    Then <permitido> podera ser permitido seguir com a compra

    Examples:
        | sku | quantidade | permitido |
        | 123 |         10 |       SIM |
        | 123 |         11 |       NAO |
        | 456 |          2 |       SIM |
        | 456 |       2000 |       NAO |
        | 321 |          1 |       SIM |
        | 321 |          2 |       NAO |
        | 654 |          2 |       SIM |
        | 654 |         20 |       SIM |