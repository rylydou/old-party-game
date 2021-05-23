@echo off

FOR /R "%CD%\..\Assets\Shaders" %%a IN (*.mgfx) DO (
	DEL "%%a"
)

FOR /R "%CD%\..\Assets\Shaders" %%a IN (*.fx) DO (
	mgfxc "%%a" "%%atmp" /Profile:OpenGL

	@REM IF %ERRORLEVEL% NEQ 0 (
	@REM 	SET /P result = "Install Shader Compiler? y/n"

	@REM 	IF %result% EQU "y" (
	@REM 		dotnet tool install dotnet-mgfxc -g
	@REM 	)
	@REM 	ELSE (
	@REM 		EXIT 0
	@REM 	)
	@REM )
)

FOR /R "%CD%\..\Assets\Shaders" %%a IN (*.fxtmp) DO (
	REN "%%a" *.mgfx
)

@REM SET /P name=".....I'm Waiting....."