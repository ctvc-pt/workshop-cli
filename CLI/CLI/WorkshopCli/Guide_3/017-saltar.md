[color=white]
Agora vamos fazer o skater saltar! Para isso precisamos
de uma nova funcao chamada "love.keypressed".

Esta funcao e chamada automaticamente sempre que carregas
numa tecla.

Adiciona uma NOVA funcao no final do ficheiro (depois de
love.draw):

[/color] [color=blue]
function love.keypressed(tecla)
    -- Saltar
    if (tecla == "space" or tecla == "up") and jogador.noChao and not fimDeJogo then
        jogador.velocidadeY = jogador.forcaSalto
        jogador.noChao = false
        jogador.agachado = false
    end
end
[/color] [color=white]

Como funciona:
- Quando carregas ESPACO ou seta CIMA, o skater salta
- "jogador.noChao" garante que so salta se estiver no chao
- "forcaSalto" e um valor negativo (-430), que empurra o
  skater para cima
- A gravidade que ja adicionamos trata de o puxar de volta

Agora vamos mudar a pose quando o skater esta no ar.
Na funcao "love.draw()", muda a parte da pose para:

[/color] [color=blue]
    -- Pose do jogador
    local poseAtual = poseAndar
    if not jogador.noChao then poseAtual = poseSaltar end
[/color] [color=white]

Executa o jogo (Alt + L) e carrega ESPACO para saltar!
[/color]

