[color=white]
Fantastico! O skater ja salta e agacha-se! Agora vamos
criar obstaculos para tornar o jogo dificil.

O primeiro obstaculo e o banco. O jogador tem de saltar
por cima dele.

Adiciona uma NOVA funcao no final do ficheiro:

[/color] [color=blue]
function criarBanco()
    table.insert(obstaculos, {
        tipo = "banco",
        x = LARGURA + 10,
        y = CHAO_Y - bancoAltura,
        largura = bancoLargura,
        altura = bancoAltura,
    })
end
[/color] [color=white]

"table.insert" adiciona um novo item a lista "obstaculos".
Cada obstaculo e uma tabela com:
- "tipo" - que tipo de obstaculo e
- "x, y" - posicao (comeca fora do ecra, a direita)
- "largura, altura" - tamanho para calcular colisoes

Agora vamos desenhar o banco! Adiciona o seguinte na
funcao "love.draw()", DEPOIS de desenhares o skater e
ANTES da interface:

[/color] [color=blue]
    -- Obstaculos
    for _, obstaculo in ipairs(obstaculos) do
        love.graphics.setColor(1, 1, 1)

        if obstaculo.tipo == "banco" then
            love.graphics.draw(imagemBanco, recorteBanco,
                obstaculo.x, obstaculo.y,
                0, escalaBanco, escalaBanco
            )
        end
    end
[/color] [color=white]

O "for" percorre todos os obstaculos e desenha cada um.
"ipairs" e uma funcao que percorre listas em Lua.

Ainda nao vais ver bancos porque nao estamos a cria-los
automaticamente. Vamos fazer isso no proximo passo!
[/color]

