[color=white]
Boa! O skater ja salta! Agora vamos adicionar a capacidade
de se agachar para desviar das gaivotas.

Adiciona o seguinte DENTRO da funcao "love.keypressed(tecla)",
DEPOIS do codigo do salto:

[/color] [color=blue]
    -- Agachar
    if tecla == "down" and jogador.noChao and not fimDeJogo then
        jogador.agachado = true
    end
[/color] [color=white]

Quando largas a tecla, o skater levanta-se. Para isso
precisamos de outra funcao nova. Adiciona-a DEPOIS de
"love.keypressed":

[/color] [color=blue]
function love.keyreleased(tecla)
    if tecla == "down" then
        jogador.agachado = false
    end
end
[/color] [color=white]

Agora vamos mudar a pose quando esta agachado.
Na funcao "love.draw()", onde tens a pose, adiciona
mais uma linha:

[/color] [color=blue]
    -- Pose do jogador
    local poseAtual = poseAndar
    if not jogador.noChao then poseAtual = poseSaltar end
    if jogador.agachado then poseAtual = poseAgachar end
[/color] [color=white]

Executa o jogo (Alt + L). Carrega ESPACO para saltar e
seta BAIXO para agachar!
[/color]

