using IntegradorIdea.Models;

using Microsoft.EntityFrameworkCore;



namespace IntegradorIdea.Data

{

    public class DataContext : DbContext

    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }



        public virtual DbSet<TComunicacaoEletronica> TComunicacaoEletronica { get; set; }

        public virtual DbSet<TConfiguracao> TConfiguracao { get; set; }

        public virtual DbSet<TFilaPastaDigital> TFilaPastaDigital { get; set; }

        public virtual DbSet<TGrupoMenu> TGrupoMenu { get; set; }

        public virtual DbSet<TLogAtividade> TLogAtividade { get; set; }

        public virtual DbSet<TLogOperacao> TLogOperacao { get; set; }

        public virtual DbSet<TPerfil> TPerfil { get; set; }

        public virtual DbSet<TPerfilTela> TPerfilTela { get; set; }

        public virtual DbSet<TServidor> TServidor { get; set; }

        public virtual DbSet<tTpSituacaoPastaDigital> tTpSituacaoPastaDigital { get; set; }

        public virtual DbSet<TTela> TTela { get; set; }

        public virtual DbSet<tTpOperacao> tTpOperacao { get; set; }

        public virtual DbSet<tTpRetorno> tTpRetorno { get; set; }

        public virtual DbSet<TUsuario> TUsuario { get; set; }

        public virtual DbSet<tTpDocumentoParte> tTpDocumentoParte { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            modelBuilder.Entity<TComunicacaoEletronica>(entity =>

            {

                entity.HasKey(e => e.IdComunicacaoEletronica)

                    .HasName("pk_tComunicacaoEletronica");



                entity.ToTable("tComunicacaoEletronica");



                entity.Property(e => e.DsCaminhoDocumentosAnexoAtoDisponibilizado)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DsCaminhoDocumentosAnexoAtoEnvio)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DsCaminhoDocumentosAnexoAtoRetorno)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DsComplemento)

                    .HasMaxLength(3500)

                    .IsUnicode(false);



                entity.Property(e => e.DsMovimentacao)

                    .HasMaxLength(4000)

                    .IsUnicode(false);



                entity.Property(e => e.NmVara)

                    .HasMaxLength(100)

                    .IsUnicode(false);



                entity.Property(e => e.DsTeorAto).HasColumnType("text");



                entity.Property(e => e.DtCiencia).HasColumnType("datetime");



                entity.Property(e => e.DtDisponibilizacao).HasColumnType("datetime");



                entity.Property(e => e.DtIntimacao).HasColumnType("datetime");



                entity.Property(e => e.DtLimiteCiencia).HasColumnType("datetime");



                entity.Property(e => e.DtMovimentacao).HasColumnType("datetime");



                entity.Property(e => e.DtRecebimento).HasColumnType("datetime");



                entity.Property(e => e.DsDocumentosAnexos)

                    .HasMaxLength(20)

                    .IsUnicode(false);



                entity.Property(e => e.DsOutrosNumeros)

                    .HasMaxLength(100)

                    .IsUnicode(false);



                entity.Property(e => e.NuProcesso)

                    .IsRequired()

                    .HasMaxLength(25)

                    .IsUnicode(false);



                entity.Property(e => e.SgTpIntimacaoCitacao)

                    .HasMaxLength(1)

                    .IsUnicode(false)

                    .IsFixedLength();

            });



            modelBuilder.Entity<TConfiguracao>(entity =>

            {

                entity.HasKey(e => e.IdConfiguracao)

                    .HasName("pk_tConfiguracao");



                entity.ToTable("tConfiguracao");



                entity.Property(e => e.DsChave)

                    .IsRequired()

                    .HasMaxLength(150)

                    .IsUnicode(false);



                entity.Property(e => e.DsValor)

                    .IsRequired()

                    .HasMaxLength(150)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.DsConfiguracao)

                    .IsRequired();

            });



            modelBuilder.Entity<TFilaPastaDigital>(entity =>

            {

                entity.HasKey(e => e.IdFilaPastaDigital)

                    .HasName("pk_tFilaPastaDigital");



                entity.ToTable("tFilaPastaDigital");



                entity.Property(e => e.CdIdea).HasColumnName("CdIDEA");



                entity.Property(e => e.DsCaminhoPastaDigital)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DsErro)

                    .HasMaxLength(25)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.DtFinal).HasColumnType("datetime");



                entity.Property(e => e.DtInicial).HasColumnType("datetime");



                entity.Property(e => e.DtInicioProcessamento).HasColumnType("datetime");



                entity.Property(e => e.NuProcesso)

                    .IsRequired()

                    .HasMaxLength(25)

                    .IsUnicode(false);



                entity.HasOne(d => d.IdServidorNavigation)

                    .WithMany(p => p.TFilaPastaDigital)

                    .HasForeignKey(d => d.IdServidor)

                    .OnDelete(DeleteBehavior.ClientSetNull)

                    .HasConstraintName("fk_tFilaPastaDigital_tServidor");



                entity.HasOne(d => d.IdSituacaoPastaDigitalNavigation)

                    .WithMany(p => p.TFilaPastaDigital)

                    .HasForeignKey(d => d.IdTpSituacaoPastaDigital)

                    .OnDelete(DeleteBehavior.ClientSetNull)

                    .HasConstraintName("fk_tFilaPastaDigital_tTpSituacaoPastaDigital");

            });



            modelBuilder.Entity<TGrupoMenu>(entity =>

            {

                entity.HasKey(e => e.IdGrupoMenu)

                    .HasName("pk_tGrupoMenu");



                entity.ToTable("tGrupoMenu");



                entity.Property(e => e.DsIcone)

                    .HasMaxLength(100)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.NmGrupoMenu)

                    .IsRequired()

                    .HasMaxLength(100)

                    .IsUnicode(false);

            });



            modelBuilder.Entity<TLogAtividade>(entity =>

            {

                entity.HasKey(e => e.IdLogAtividade)

                    .HasName("pk_tLogAtividade");



                entity.ToTable("tLogAtividade");



                entity.Property(e => e.DsDados).HasColumnType("text");



                entity.Property(e => e.DsDispositivo)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DsIp)

                    .IsRequired()

                    .HasMaxLength(40)

                    .IsUnicode(false);



                entity.Property(e => e.DtLog)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.HasOne(d => d.IdTelaNavigation)

                    .WithMany(p => p.TLogAtividade)

                    .HasForeignKey(d => d.IdTela)

                    .OnDelete(DeleteBehavior.ClientSetNull)

                    .HasConstraintName("fk_tLogAtividade_tTela");



                entity.HasOne(d => d.IdUsuarioNavigation)

                    .WithMany(p => p.TLogAtividade)

                    .HasForeignKey(d => d.IdUsuario)

                    .OnDelete(DeleteBehavior.ClientSetNull)

                    .HasConstraintName("fk_tLogAtividade_tUsuario");

            });



            modelBuilder.Entity<TLogOperacao>(entity =>

            {

                entity.HasKey(e => e.IdLogOperacao)

                    .HasName("pk_tLogOperacao");



                entity.ToTable("tLogOperacao");



                entity.Property(e => e.CdIdea)

                    .HasColumnName("CdIDEA");



                entity.Property(e => e.DsCaminhoDocumentosChamada)

                    .IsRequired()

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DsCaminhoDocumentosRetorno)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DsIpdestino)

                    .IsRequired()

                    .HasColumnName("DsIPDestino")

                    .HasMaxLength(40)

                    .IsUnicode(false);



                entity.Property(e => e.DsIporigem)

                    .IsRequired()

                    .HasColumnName("DsIPOrigem")

                    .HasMaxLength(40)

                    .IsUnicode(false);



                entity.Property(e => e.DsLogOperacao)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DtFinalOperacao).HasColumnType("datetime");



                entity.Property(e => e.DtInicioOperacao).HasColumnType("datetime");



                entity.Property(e => e.DtLogOperacao)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.HasOne(d => d.IdTipoOperacaoNavigation)

                    .WithMany(p => p.TLogOperacao)

                    .HasForeignKey(d => d.IdTpOperacao)

                    .OnDelete(DeleteBehavior.ClientSetNull)

                    .HasConstraintName("fk_tLogOperacao_tTpOperacao");



                entity.HasOne(d => d.IdTipoRetornoNavigation)

                    .WithMany(p => p.TLogOperacao)

                    .HasForeignKey(d => d.IdTpRetorno)

                    .OnDelete(DeleteBehavior.ClientSetNull)

                    .HasConstraintName("fk_tLogOperacao_tTpRetorno");

            });



            modelBuilder.Entity<TPerfil>(entity =>

            {

                entity.HasKey(e => e.IdPerfil)

                    .HasName("pk_tPerfil");



                entity.ToTable("tPerfil");



                entity.Property(e => e.DsPerfil)

                    .IsRequired()

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.NmPerfil)

                    .IsRequired()

                    .HasMaxLength(100)

                    .IsUnicode(false);

            });



            modelBuilder.Entity<TPerfilTela>(entity =>

            {

                entity.HasKey(e => e.IdPerfilTela)

                    .HasName("pk_tPerfilTela");



                entity.ToTable("tPerfilTela");



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.HasOne(d => d.IdPerfilNavigation)

                    .WithMany(p => p.TPerfilTela)

                    .HasForeignKey(d => d.IdPerfil)

                    .HasConstraintName("fk_tPerfilTela_tPerfil");



                entity.HasOne(d => d.IdTelaNavigation)

                    .WithMany(p => p.TPerfilTela)

                    .HasForeignKey(d => d.IdTela)

                    .HasConstraintName("fk_tPerfilTela_tTela");

            });



            modelBuilder.Entity<TServidor>(entity =>

            {

                entity.HasKey(e => e.IdServidor)

                    .HasName("pk_tServidor");



                entity.ToTable("tServidor");



                entity.Property(e => e.DsEnderecoWsdl)

                    .IsRequired()

                    .HasColumnName("DsEnderecoWSDL")

                    .HasMaxLength(250)

                    .IsUnicode(false);



                entity.Property(e => e.DsIpservidor)

                    .IsRequired()

                    .HasColumnName("DsIPServidor")

                    .HasMaxLength(40)

                    .IsUnicode(false);



                entity.Property(e => e.DsServidor)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.DtUltimaReinicializacao).HasColumnType("datetime");



                entity.Property(e => e.SgFlSituacao)

                    .HasMaxLength(1)

                    .IsUnicode(false)

                    .IsFixedLength();



                entity.Property(e => e.NmServidor)

                    .IsRequired()

                    .HasMaxLength(50)

                    .IsUnicode(false);

            });



            modelBuilder.Entity<tTpSituacaoPastaDigital>(entity =>

            {

                entity.HasKey(e => e.IdTpSituacaoPastaDigital)

                    .HasName("pk_tSituacaoPastaDigital");



                entity.ToTable("tTpSituacaoPastaDigital");



                entity.Property(e => e.DsCor)

                    .IsRequired()

                    .HasMaxLength(10)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.NmTpSituacaoPastaDigital)

                    .IsRequired()

                    .HasMaxLength(50)

                    .IsUnicode(false);

            });



            modelBuilder.Entity<TTela>(entity =>

            {

                entity.HasKey(e => e.IdTela)

                    .HasName("pk_tTela");



                entity.ToTable("tTela");



                entity.Property(e => e.DsAcao)

                    .IsRequired()

                    .HasMaxLength(100)

                    .IsUnicode(false);



                entity.Property(e => e.DsControlador)

                    .IsRequired()

                    .HasMaxLength(100)

                    .IsUnicode(false);



                entity.Property(e => e.DsIcone)

                    .HasMaxLength(100)

                    .IsUnicode(false);



                entity.Property(e => e.DsTela)

                    .IsRequired()

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.FlRegistrarLog)

                    .IsRequired()

                    .HasDefaultValueSql("((1))");



                entity.Property(e => e.NmTela)

                    .IsRequired()

                    .HasMaxLength(100)

                    .IsUnicode(false);



                entity.HasOne(d => d.IdGrupoMenuNavigation)

                    .WithMany(p => p.TTela)

                    .HasForeignKey(d => d.IdGrupoMenu)

                    .HasConstraintName("fk_tTela_tGrupoMenu");

            });



            modelBuilder.Entity<tTpOperacao>(entity =>

            {

                entity.HasKey(e => e.IdTpOperacao)

                    .HasName("pk_tTpOperacao");



                entity.ToTable("tTpOperacao");



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.NmTpOperacao)

                    .IsRequired()

                    .HasMaxLength(100)

                    .IsUnicode(false);

            });



            modelBuilder.Entity<tTpRetorno>(entity =>

            {

                entity.HasKey(e => e.IdTpRetorno)

                    .HasName("pk_tTpRetorno");



                entity.ToTable("tTpRetorno");



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.NmTpRetorno)

                    .IsRequired()

                    .HasMaxLength(100)

                    .IsUnicode(false);

            });



            modelBuilder.Entity<TUsuario>(entity =>

            {

                entity.HasKey(e => e.IdUsuario)

                    .HasName("pk_tUsuario");



                entity.ToTable("tUsuario");



                entity.Property(e => e.DsLogin)

                    .IsRequired()

                    .HasMaxLength(150)

                    .IsUnicode(false);



                entity.Property(e => e.DsSenha)

                    .HasMaxLength(150)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");



                entity.Property(e => e.DtUltimoAcesso).HasColumnType("datetime");



                entity.Property(e => e.NmUsuario)

                    .IsRequired()

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.HasOne(d => d.IdPerfilNavigation)

                    .WithMany(p => p.TUsuario)

                    .HasForeignKey(d => d.IdPerfil)

                    .OnDelete(DeleteBehavior.ClientSetNull)

                    .HasConstraintName("fk_tUsuario_tPerfil");

            });



            modelBuilder.Entity<tTpDocumentoParte>(entity =>

            {

                entity.HasKey(e => e.IdTpDocumentoParte)

                    .HasName("pk_tTpDocumentoParte");



                entity.ToTable("tTpDocumentoParte");



                entity.Property(e => e.SgTpDocumentoPje)

                    .IsRequired()

                    .HasMaxLength(150)

                    .IsUnicode(false);



                entity.Property(e => e.SgTpDocumentoEsaj)

                    .IsRequired()

                    .HasMaxLength(150)

                    .IsUnicode(false);



                entity.Property(e => e.DsEmissorDocumento)

                    .HasMaxLength(255)

                    .IsUnicode(false);



                entity.Property(e => e.DtCadastro)

                    .HasColumnType("datetime")

                    .HasDefaultValueSql("(getdate())");

            });

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {

            optionsBuilder.EnableSensitiveDataLogging();

        }



    }

}