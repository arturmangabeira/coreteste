using Core.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Core.Api.Data
{
    public class DataContext : DbContext 
    {        
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public virtual DbSet<TComunicacaoEletronica> TComunicacaoEletronica { get; set; }
        public virtual DbSet<TConfiguracao> TConfiguracao { get; set; }
        public virtual DbSet<TFilaPastaDigital> TFilaPastaDigital { get; set; }
        public virtual DbSet<TGrupoMenu> TGrupoMenu { get; set; }
        public virtual DbSet<TLogAtividade> TLogAtividade { get; set; }
        public virtual DbSet<TLogOperacao> TLogOperacao { get; set; }
        public virtual DbSet<TPerfil> TPerfil { get; set; }
        public virtual DbSet<TPerfilTela> TPerfilTela { get; set; }
        public virtual DbSet<TServidor> TServidor { get; set; }
        public virtual DbSet<TSituacaoPastaDigital> TSituacaoPastaDigital { get; set; }
        public virtual DbSet<TTela> TTela { get; set; }
        public virtual DbSet<TTipoOperacao> TTipoOperacao { get; set; }
        public virtual DbSet<TTipoRetorno> TTipoRetorno { get; set; }
        public virtual DbSet<TUsuario> TUsuario { get; set; }
        public virtual DbSet<TTipoDocumentoParte> TTipoDocumentoParte { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TComunicacaoEletronica>(entity =>
            {
                entity.HasKey(e => e.IdComunicacaoEletronica)
                    .HasName("pk_tComunicacaoEletronica");

                entity.ToTable("tComunicacaoEletronica");

                entity.Property(e => e.DsCaminhoDocumentosAnexoAtoAssinado)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DsCaminhoDocumentosAnexoAtoEnviados)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DsCaminhoDocumentosAnexoAtoRecebidos)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DsComplemento)
                    .HasMaxLength(3500)
                    .IsUnicode(false);

                entity.Property(e => e.DsMovimentacao)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.DsTeorAto).HasColumnType("text");

                entity.Property(e => e.DtCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtCiencia).HasColumnType("datetime");

                entity.Property(e => e.DtDisponibilizacao).HasColumnType("datetime");

                entity.Property(e => e.DtIntimacao).HasColumnType("datetime");

                entity.Property(e => e.DtLimiteCiencia).HasColumnType("datetime");

                entity.Property(e => e.DtMovimentacao).HasColumnType("datetime");

                entity.Property(e => e.DtRecebimento).HasColumnType("datetime");

                entity.Property(e => e.FlRecebimentoAutomatico)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NuDocumentosAnexos)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NuOutrosNumeros)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NuProcesso)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.TpIntimacaoCitacao)
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

                entity.Property(e => e.TxDescricao)
                    .IsRequired()
                    .HasColumnType("text");
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
                    .HasForeignKey(d => d.IdSituacaoPastaDigital)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tFilaPastaDigital_tSituacaoPastaDigital");
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
                    .HasForeignKey(d => d.IdTipoOperacao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tLogOperacao_tTipoOperacao");

                entity.HasOne(d => d.IdTipoRetornoNavigation)
                    .WithMany(p => p.TLogOperacao)
                    .HasForeignKey(d => d.IdTipoRetorno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tLogOperacao_tTipoRetorno");
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

                entity.HasOne(d => d.IdTelaNavigation)
                    .WithMany(p => p.TPerfil)
                    .HasForeignKey(d => d.IdTela)
                    .HasConstraintName("fk_tPerfil_tTela");
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

                entity.Property(e => e.FlSituacao)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NmServidor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TSituacaoPastaDigital>(entity =>
            {
                entity.HasKey(e => e.IdSituacaoPastaDigital)
                    .HasName("pk_tSituacaoPastaDigital");

                entity.ToTable("tSituacaoPastaDigital");

                entity.Property(e => e.DsCor)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DtCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NmSituacaoPastaDigital)
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

            modelBuilder.Entity<TTipoOperacao>(entity =>
            {
                entity.HasKey(e => e.IdTipoOperacao)
                    .HasName("pk_tTipoOperacao");

                entity.ToTable("tTipoOperacao");

                entity.Property(e => e.DtCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NmTipoOperacao)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TTipoRetorno>(entity =>
            {
                entity.HasKey(e => e.IdTipoRetorno)
                    .HasName("pk_tTipoRetorno");

                entity.ToTable("tTipoRetorno");

                entity.Property(e => e.DtCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NmTipoRetorno)
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

            modelBuilder.Entity<TTipoDocumentoParte>(entity =>
            {
                entity.HasKey(e => e.IdTipoDocumentoParte)
                    .HasName("pk_tTipoDocumentoParte");

                entity.ToTable("tTipoDocumentoParte");
                
                entity.Property(e => e.SgTipoDocumentoPje)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.SgTipoDocumentoEsaj)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DsDescricaoEmissorDocumento)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DtCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });
        }

        protected  override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}