using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Models.Entities;

public partial class ChipinDbContext : DbContext
{
    public ChipinDbContext()
    {
    }

    public ChipinDbContext(DbContextOptions<ChipinDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<BillingAddress> BillingAddresses { get; set; }

    public virtual DbSet<ExternalProduct> ExternalProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductListItem> ProductListItems { get; set; }

    public virtual DbSet<ProductListWallet> ProductListWallets { get; set; }

    public virtual DbSet<ProductListWalletTransaction> ProductListWalletTransactions { get; set; }

    public virtual DbSet<Salt> Salts { get; set; }

    public virtual DbSet<ShareList> ShareLists { get; set; }

    public virtual DbSet<TokenWallet> TokenWallets { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    => optionsBuilder.UseMySQL("server=chipinmysqldbserver.mysql.database.azure.com;port=3306;uid=rob;pwd=Chipininvent2022;database=chipin_db");
    //     => optionsBuilder.UseMySQL("server=localhost;port=3306;uid=root;pwd=Jayden52;database=chipin_db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PRIMARY");

            entity.ToTable("address");

            entity.HasIndex(e => e.ChipinId, "fk_address_user_table1_idx");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.Address1)
                .HasMaxLength(255)
                .HasColumnName("address_1");
            entity.Property(e => e.Address2)
                .HasMaxLength(255)
                .HasColumnName("address_2");
            entity.Property(e => e.AdressName)
                .HasMaxLength(255)
                .HasColumnName("adress_name");
            entity.Property(e => e.ChipinId).HasColumnName("chipin_id");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(45)
                .HasColumnName("phone_number");
            entity.Property(e => e.PostCode)
                .HasMaxLength(255)
                .HasColumnName("post_code");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .HasColumnName("state");

            entity.HasOne(d => d.Chipin).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.ChipinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_address_user_table1");
        });

        modelBuilder.Entity<BillingAddress>(entity =>
        {
            entity.HasKey(e => e.BillingAddressId).HasName("PRIMARY");

            entity.ToTable("billing_address");

            entity.HasIndex(e => e.ChipinId, "chipin_id_idx");

            entity.Property(e => e.BillingAddressId).HasColumnName("billing_address_id");
            entity.Property(e => e.Address1)
                .HasMaxLength(255)
                .HasColumnName("address_1");
            entity.Property(e => e.Address2)
                .HasMaxLength(255)
                .HasColumnName("address_2");
            entity.Property(e => e.AdressName)
                .HasMaxLength(255)
                .HasColumnName("adress_name");
            entity.Property(e => e.ChipinId).HasColumnName("chipin_id");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(45)
                .HasColumnName("phone_number");
            entity.Property(e => e.PostCode)
                .HasMaxLength(255)
                .HasColumnName("post_code");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .HasColumnName("state");

            entity.HasOne(d => d.Chipin).WithMany(p => p.BillingAddresses)
                .HasForeignKey(d => d.ChipinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_chipin_id.usertable");
        });

        modelBuilder.Entity<ExternalProduct>(entity =>
        {
            entity.HasKey(e => e.ExternalProductId).HasName("PRIMARY");

            entity.ToTable("external_products");

            entity.Property(e => e.ExternalProductId).HasColumnName("external_product_id");
            entity.Property(e => e.CustInt1).HasColumnName("cust_int1");
            entity.Property(e => e.CustInt2).HasColumnName("cust_int2");
            entity.Property(e => e.CustInt3).HasColumnName("cust_int3");
            entity.Property(e => e.CustString1)
                .HasMaxLength(255)
                .HasColumnName("cust_string1");
            entity.Property(e => e.CustString2)
                .HasMaxLength(255)
                .HasColumnName("cust_string2");
            entity.Property(e => e.CustString3)
                .HasMaxLength(255)
                .HasColumnName("cust_string3");
            entity.Property(e => e.Image)
                .HasColumnType("text")
                .HasColumnName("image");
            entity.Property(e => e.Image1)
                .HasColumnType("text")
                .HasColumnName("image1");
            entity.Property(e => e.Image2)
                .HasColumnType("text")
                .HasColumnName("image2");
            entity.Property(e => e.Image3)
                .HasColumnType("text")
                .HasColumnName("image3");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ProductId)
                .HasColumnType("text")
                .HasColumnName("product_id");
            entity.Property(e => e.PageUrl)
                .HasColumnType("text")
                .HasColumnName("page_url");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductDescription)
                .HasColumnType("text")
                .HasColumnName("product_description");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("product_name");
            entity.Property(e => e.ReturnUrl)
                .HasColumnType("text")
                .HasColumnName("return_url");
            entity.Property(e => e.Store)
                .HasMaxLength(255)
                .HasColumnName("store");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductDescription)
                .HasColumnType("text")
                .HasColumnName("product_description");
            entity.Property(e => e.ProductImage)
                .HasColumnType("text")
                .HasColumnName("product_image");
            entity.Property(e => e.ProductImage1)
                .HasColumnType("text")
                .HasColumnName("product_image1");
            entity.Property(e => e.ProductImage2)
                .HasColumnType("text")
                .HasColumnName("product_image2");
            entity.Property(e => e.ProductImage3)
                .HasColumnType("text")
                .HasColumnName("product_image3");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Store)
                .HasMaxLength(255)
                .HasColumnName("store");
        });

        modelBuilder.Entity<ProductListItem>(entity =>
        {
            entity.HasKey(e => e.ChipinProductListEntryId).HasName("PRIMARY");

            entity.ToTable("product_list_items");

            entity.HasIndex(e => e.ExternalProductId, "fk_product_list_items_external_products1_idx");

            entity.HasIndex(e => e.ProductListWalletId, "fk_product_list_items_product_list_wallet1_idx");

            entity.HasIndex(e => e.ProductId, "fk_product_list_products1_idx");

            entity.Property(e => e.ChipinProductListEntryId).HasColumnName("chipin_product_list_entry_id");
            entity.Property(e => e.ExternalProductId).HasColumnName("external_product_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductListWalletId).HasColumnName("product_list_wallet_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.ExternalProduct).WithMany(p => p.ProductListItems)
                .HasForeignKey(d => d.ExternalProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_product_list_items_external_products1");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductListItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_product_list_items_products1");

            entity.HasOne(d => d.ProductListWallet).WithMany(p => p.ProductListItems)
                .HasForeignKey(d => d.ProductListWalletId)
                .HasConstraintName("fk_product_list_items_product_list_wallet1");
        });

        modelBuilder.Entity<ProductListWallet>(entity =>
        {
            entity.HasKey(e => e.ProductListWalletId).HasName("PRIMARY");

            entity.ToTable("product_list_wallet");

            entity.HasIndex(e => e.AddressId, "fk_product_list_wallet_address1_idx");

            entity.HasIndex(e => e.ChipinId, "fk_product_list_wallet_user_table1_idx");

            entity.Property(e => e.ProductListWalletId).HasColumnName("product_list_wallet_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.ChipinId).HasColumnName("chipin_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EndAt)
                .HasColumnType("datetime")
                .HasColumnName("end_at");
            entity.Property(e => e.Funded).HasColumnName("funded");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.Closed).HasColumnName("closed");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Address).WithMany(p => p.ProductListWallets)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("fk_product_list_wallet_address1");

            entity.HasOne(d => d.Chipin).WithMany(p => p.ProductListWallets)
                .HasForeignKey(d => d.ChipinId)
                .HasConstraintName("fk_product_list_wallet_user_table1");
        });

        /* modelBuilder.Entity<TempConnection>(entity =>
         {
             entity.HasKey(e => e.TempConnectionId).HasName("PRIMARY");
             entity.HasIndex(e => e.ChipinId, "fk_temp_connection_user_table1_idx");
             entity.Property(e => e.TempConnectionId).HasColumnName("temp_connection_id");

             entity.ToTable("temp_connection");

             entity.Property(e => e.ChipinId)
                 .HasColumnName("chipin_id");

             entity.Property(e => e.SessionId)
                 .HasMaxLength(45)
                 .HasColumnName("session_id");

             entity.Property(e => e.CreatedAt)
                 .HasColumnType("datetime")
                 .HasColumnName("created_at");

             entity.Property(e => e.ReturnUrl)
                 .HasColumnType("text")
                 .HasColumnName("return_url");

             entity.HasOne(d => d.Chipin).WithMany(p => p.TempConnections)
                 .HasForeignKey(d => d.ChipinId)
                 .HasConstraintName("fk_chipin_id_usertable");
         });*/

        modelBuilder.Entity<ProductListWalletTransaction>(entity =>
        {
            entity.HasKey(e => e.ProductListWalletTransactionId).HasName("PRIMARY");

            entity.ToTable("product_list_wallet_transaction");

            entity.HasIndex(e => e.ProductListWalletId, "fk_product_list_wallet_transaction_product_list_wallet1_idx");

            entity.Property(e => e.ProductListWalletTransactionId).HasColumnName("product_list_wallet_transaction_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.ChipinId).HasColumnName("chipin_id");
            entity.Property(e => e.FromInvitedUser).HasColumnName("from_invited_user");
            entity.Property(e => e.ProductListWalletId).HasColumnName("product_list_wallet_id");
            entity.Property(e => e.TransactionMethod)
                .HasMaxLength(255)
                .HasColumnName("transaction_method");
            //entity.Property(e => e.TransactionId).HasMaxLength(255).HasColumnName("transaction_id");
            //entity.Property(e => e.PaymentKey).HasMaxLength(255).HasColumnName("payment_key");
          //  entity.Property(e => e.CreatedAt)
           ///     .HasColumnType("datetime")
           //     .HasColumnName("created_at");

            entity.HasOne(d => d.ProductListWallet).WithMany(p => p.ProductListWalletTransactions)
                .HasForeignKey(d => d.ProductListWalletId)
                .HasConstraintName("fk_product_list_wallet_transaction_product_list_wallet1");
        });

        modelBuilder.Entity<Salt>(entity =>
        {
            entity.HasKey(e => e.SaltId).HasName("PRIMARY");

            entity.ToTable("salt");

            entity.HasIndex(e => e.ChipinId, "fk_salt_user_table1_idx");

            entity.Property(e => e.SaltId).HasColumnName("salt_id");
            entity.Property(e => e.ChipinId).HasColumnName("chipin_id");
            entity.Property(e => e.Salt1)
                .HasMaxLength(255)
                .HasColumnName("salt");

            entity.HasOne(d => d.Chipin).WithMany(p => p.Salts)
                .HasForeignKey(d => d.ChipinId)
                .HasConstraintName("fk_salt_user_table1");
        });

        modelBuilder.Entity<ShareList>(entity =>
        {
            entity.HasKey(e => e.ShareId).HasName("PRIMARY");

            entity.ToTable("share_list");

            entity.HasIndex(e => e.ProductListWalletTransactionId, "fk_share_list_product_list_wallet_transaction1_idx");

            entity.Property(e => e.ShareId).HasColumnName("share_id");
            entity.Property(e => e.ProductListWalletTransactionId).HasColumnName("product_list_wallet_transaction_id");
            entity.Property(e => e.ShareDate)
                .HasColumnType("datetime")
                .HasColumnName("share_date");
            entity.Property(e => e.ShareEmail)
                .HasMaxLength(255)
                .HasColumnName("share_email");
            entity.Property(e => e.ShareMessage)
                .HasColumnType("text")
                .HasColumnName("share_message");
            entity.Property(e => e.ShareName)
                .HasMaxLength(255)
                .HasColumnName("share_name");

            entity.HasOne(d => d.ProductListWalletTransaction).WithMany(p => p.ShareLists)
                .HasForeignKey(d => d.ProductListWalletTransactionId)
                .HasConstraintName("fk_share_list_product_list_wallet_transaction1");
        });

        modelBuilder.Entity<TokenWallet>(entity =>
        {
            entity.HasKey(e => e.TokenWalletId).HasName("PRIMARY");

            entity.ToTable("token_wallet");

            entity.HasIndex(e => e.ChipinId, "fk_token_wallet_user_table1_idx");

            entity.Property(e => e.TokenWalletId).HasColumnName("token_wallet_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.ChipinId).HasColumnName("chipin_id");

            entity.HasOne(d => d.Chipin).WithMany(p => p.TokenWallets)
                .HasForeignKey(d => d.ChipinId)
                .HasConstraintName("fk_token_wallet_user_table1");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.ChipinId).HasName("PRIMARY");

            entity.ToTable("user_table");

            entity.Property(e => e.ChipinId).HasColumnName("chipin_id");
            entity.Property(e => e.ChipinCreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("chipin_created_date");
            entity.Property(e => e.ChipinName)
                .HasMaxLength(45)
                .HasColumnName("chipin_name");
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .HasColumnName("salt");
            entity.Property(e => e.TokenWalletId).HasColumnName("token_wallet_id");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .HasColumnName("user_email");
            entity.Property(e => e.UserPass)
                .HasMaxLength(255)
                .HasColumnName("user_pass");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    //public DbSet<Chipin_Rewrite.Models.woocommerce.TempConnection> TempConnection { get; set; } = default!;
}
